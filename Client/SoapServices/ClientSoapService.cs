using OnlineStore.Client.Models.Accounts;
using OnlineStore.Client.Services;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.SoapServices;

public class ClientSoapService(ISoapClient soapClient) : IClientService
{
    private const string Endpoint = "/soap/client";
    private const string ServiceNamespace = "http://tempuri.org/IClientSoapService";
    
    public Task Register(RegisterClient command)
        => soapClient.SendCommand(Endpoint, ServiceNamespace, "RegisterUser", command);

    public Task ChangeUserPreferences(ChangePreferencesModel model)
        => Task.CompletedTask;

    public async Task UpsertOrderAddress(UpsertAddressModel model)
    {
        var command = new UpsertOrderAddress(
            id: model.Id,
            street: model.Street,
            streetNumber: model.StreetNumber,
            city: model.City,
            state: model.State,
            postalCode: model.PostalCode,
            country: model.Country);

        await soapClient.SendCommand(Endpoint, ServiceNamespace, "UpsertOrderAddress", command);
    }

    public async Task<UpsertAddressModel?> GetOrderAddress()
    {
        var addressDto = await soapClient.SendQuery<GetOrderAddress, OrderAddressDto?>(
            endpoint: "/soap/client",
            serviceNamespace: "http://tempuri.org/IClientSoapService",
            actionName: "GetOrderAddress",
            payload: null);

        if (addressDto is null)
        {
            return null;
        }

        return new UpsertAddressModel
        {
            Id = addressDto.Id,
            City = addressDto.City,
            Country = addressDto.Country,
            PostalCode = addressDto.PostalCode,
            State = addressDto.State,
            Street = addressDto.Street,
            StreetNumber = addressDto.StreetNumber
        };
    }
}
