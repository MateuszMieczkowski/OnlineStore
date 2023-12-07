using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OnlineStore.Client.Providers;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker : IApiBroker
{
    private readonly ApiAuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;

    public ApiBroker(HttpClient httpClient, ApiAuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<T> GetAsync<T>(string relativeUrl)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.GetAsync(relativeUrl);
        await Validate(response);

        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json))
        {
            return default!;
        } 
        
        var result = JsonConvert.DeserializeObject<T>(json);
        return result;
    }

    public async Task<bool> PostAsync<T>(string relativeUrl, T content)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.PostAsJsonAsync(relativeUrl, content);
        return await Validate(response);
    }

    public async Task<TDto> PostAsync<TPostDto, TDto>(string relativeUrl, TPostDto content)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.PostAsJsonAsync(relativeUrl, content);
        await Validate(response);

        return await response.Content.ReadFromJsonAsync<TDto>();
    }

    public async Task<bool> PutAsync<T>(string relativeUrl, T content)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.PutAsJsonAsync(relativeUrl, content);
        return await Validate(response);
    }

    public async Task PutAsync(string relativeUrl)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.PutAsync(relativeUrl, null);
        await Validate(response);
    }

    public async Task<TDto> PutAsync<TPutDto, TDto>(string relativeUrl, TPutDto content)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.PutAsJsonAsync(relativeUrl, content);
        await Validate(response);

        return await response.Content.ReadFromJsonAsync<TDto>();
    }

    public async Task<bool> DeleteAsync(string relativeUrl)
    {
        await IncludeAuthenticationToken();
        var response = await _httpClient.DeleteAsync(relativeUrl);

        return await Validate(response);
    }

    private async Task<bool> Validate(HttpResponseMessage? response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var exceptionDetails = await response.Content.ReadFromJsonAsync<ErrorMessageDetails>();

            throw new Exception(exceptionDetails?.Message);
        }

        return true;
    }

    private async Task IncludeAuthenticationToken()
    {
        var token = await _authenticationStateProvider.GetAuthenticationJwtToken();
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}

public record ErrorMessageDetails(string ExceptionType, int StatusCode, string Message);
