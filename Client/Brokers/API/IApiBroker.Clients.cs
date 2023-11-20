using OnlineStore.Shared.Clients;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task RegisterClientAsync(RegisterClient command);
    
    Task ChangeClientPreferences(ChangeUserPreferences command);
} 