using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ClientsRelativeUrl = "api/clients";

    public async Task RegisterClientAsync(RegisterClient command)
    {
        var commandUrl = $"{ClientsRelativeUrl}/register";
        await PostAsync(commandUrl, command);
    }

    public async Task ChangeClientPreferences(ChangeUserPreferences command)
    {
        var commandUrl = $"{ClientsRelativeUrl}/change-user-preferences";
        await PutAsync(commandUrl, command);
    }
    
    public async Task UpsertOrderAddress(UpsertOrderAddress command)
    {
        var commandUrl = $"{ClientsRelativeUrl}/order-address";
        await PutAsync(commandUrl, command);
    }

    public async Task<OrderAddressDto?> GetOrderAddress()
        => await GetAsync<OrderAddressDto?>($"{ClientsRelativeUrl}/order-address");
}
