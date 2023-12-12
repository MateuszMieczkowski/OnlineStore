using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task RegisterClientAsync(RegisterClient command);
    
    Task ChangeClientPreferences(ChangeUserPreferences command);

    Task UpsertOrderAddress(UpsertOrderAddress command);
    Task<OrderAddressDto?> GetOrderAddress();
} 