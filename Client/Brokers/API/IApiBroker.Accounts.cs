using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<AuthResponse> LoginAsync(AuthenticateUser authenticateUser);
    Task RegisterAsync(RegisterUser register);
}