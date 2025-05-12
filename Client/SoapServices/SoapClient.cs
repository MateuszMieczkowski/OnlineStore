using OnlineStore.Client.Providers;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OnlineStore.Client.SoapServices;

public interface ISoapClient
{
    Task<int> SendCommand<TCommand>(string endpoint, string serviceNamespace, string actionName, TCommand? payload, bool isCreateCommand = false);

    Task<TResponse> SendQuery<TQuery, TResponse>(string endpoint, string serviceNamespace, string actionName, TQuery? payload)
        where TResponse: class;
}

public class SoapClient(ApiAuthenticationStateProvider authenticationStateProvider, HttpClient httpClient) : ISoapClient
{
    public async Task<int> SendCommand<TCommand>(
        string endpoint,
        string serviceNamespace,
        string actionName,
        TCommand? payload,
        bool isCreateCommand = false)
    {
        var request = await CreateRequest(serviceNamespace, actionName, payload: payload, isCommand: true);

        var response = await httpClient.PostAsync(endpoint, request);
        await ThrowIfNotSucceeded(response);

        return await GetCreatedIdIfNeeded(response, isCreateCommand);
    }

    public async Task<TResponse> SendQuery<TQuery, TResponse>(string endpoint, string serviceNamespace, string actionName, TQuery? payload)
        where TResponse : class
    {
        var request = await CreateRequest(serviceNamespace, actionName, payload: payload, isCommand: false);

        var response = await httpClient.PostAsync(endpoint, request);
        await ThrowIfNotSucceeded(response);

        var soapResponse = await response.Content.ReadAsStringAsync();
        return DeserializeSoapResult<TResponse>(soapResponse);
    }
    private async Task<StringContent> CreateRequest<TRequest>(string serviceNamespace, string actionName, TRequest? payload, bool isCommand)
    {
        var envelope = BuildEnvelope(actionName, payload, isCommand);
        var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
        
        content.Headers.Clear();
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml");
        content.Headers.Add("SOAPAction", $"\"{serviceNamespace}/{actionName}\"");

        var token = await authenticationStateProvider.GetAuthenticationJwtToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        return content;
    }

    private string BuildEnvelope<T>(string actionName, T? payload, bool isCommand)
    {
        var objectNamespace = SoapActionReader.GetDataContractNamespace(typeof(T));
        var sb = new StringBuilder();
        using (var writer = XmlWriter.Create(
                   sb,
                   new XmlWriterSettings
                   {
                       OmitXmlDeclaration = true
                   }))
        {
            // Envelope
            writer.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            writer.WriteAttributeString("xmlns", "tem", null, "http://tempuri.org/");
            writer.WriteAttributeString("xmlns", "onl", null, objectNamespace);

            // Header
            writer.WriteStartElement("soapenv", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
            writer.WriteEndElement(); // </Header>

            // Body
            writer.WriteStartElement("soapenv", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            writer.WriteStartElement("tem", actionName, ns: null); // <tem:Login>
            writer.WriteStartElement("tem", isCommand ? "command" : "query", ns: null); // <tem:query/command>

            if (payload is not null)
            {
                var rawXml = SerializeObjectWithoutRoot(payload, objectNamespace);
                writer.WriteRaw(rawXml);
            }

            writer.WriteEndElement(); // </tem:query>
            writer.WriteEndElement(); // </tem:Login>
            writer.WriteEndElement(); // </soapenv:Body>
            writer.WriteEndElement(); // </soapenv:Envelope>
        }

        return sb.ToString();
    }

    private string SerializeObjectWithoutRoot<T>(T obj, string objectNamespace)
    {
        var serializer = new XmlSerializer(
            typeof(T),
            new XmlRootAttribute
            {
                ElementName = typeof(T).Name,
                Namespace = objectNamespace
            });

        var ns = new XmlSerializerNamespaces();
        ns.Add("onl", objectNamespace);

        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(
            stringWriter,
            new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            });
        serializer.Serialize(xmlWriter, obj, ns);

        var fullXml = stringWriter.ToString();

        var doc = new XmlDocument();
        doc.LoadXml(fullXml);
        return doc.DocumentElement?.InnerXml ?? string.Empty;
    }
    
    public static T DeserializeSoapResult<T>(string soapXml) where T: class
    {
        var doc = XDocument.Parse(soapXml);

        // Znajdź element końcowy - np. LoginResult
        var body = doc.Root?.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/"));
        if (body == null)
            throw new InvalidOperationException("SOAP Body not found.");

        // Znajdź pierwszy element końcowy w Body (np. LoginResponse)
        var resultNode = body.Descendants().FirstOrDefault(x => x.Name.LocalName.EndsWith("Result"));
        if (resultNode == null)
        {
            return default;
        }

        // Usuń namespace'y wewnątrz, jeśli trzeba
        var cleanedXml = RemoveAllNamespaces(resultNode.ToString());

        // Deserializuj do obiektu T
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(cleanedXml);
        return (T?)serializer.Deserialize(reader)!;
    }

    private static string RemoveAllNamespaces(string xml)
    {
        var xdoc = XDocument.Parse(xml);

        XElement RemoveNs(XElement e) =>
            new XElement(e.Name.LocalName,
                e.HasElements
                    ? e.Elements().Select(RemoveNs)
                    : (object?)e.Value
            );

        return RemoveNs(xdoc.Root!).ToString();
    }

    private async Task ThrowIfNotSucceeded(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        
        var soapFault = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(soapFault);
        XNamespace s = "http://schemas.xmlsoap.org/soap/envelope/";

        var errorMessage = doc
            .Root?
            .Element(s + "Body")?
            .Element(s + "Fault")?
            .Element("faultstring")?
            .Value ?? "Nieznany błąd";

        throw new Exception(errorMessage);
    }
    
    private static async Task<int> GetCreatedIdIfNeeded(HttpResponseMessage response, bool isCreateCommand)
    {
        if (!isCreateCommand)
        {
            return 0;
        }

        var soap = await response.Content.ReadAsStringAsync();
        var createdIdString = XDocument.Parse(soap)
            .Descendants()
            .FirstOrDefault(e => 
                e.Name.LocalName.EndsWith("Result") &&
                e.Parent != null &&
                e.Parent.Name.LocalName.EndsWith("Response"))
            ?.Value;
        
        return int.Parse(createdIdString ?? "0");
    }
}