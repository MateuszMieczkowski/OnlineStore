using OnlineStore.Shared.Accounts;
using System.Net.Http.Json;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string AccountRelativeUrl = "api/account";

    public async Task<AuthResponse> LoginAsync(AuthenticateUser authenticateUser)
    {
        var response = await _httpClient.PostAsJsonAsync(AccountRelativeUrl + "/login", authenticateUser);

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();

        return auth;
    }

    public async Task RegisterAsync(RegisterUser register)
    {
        await PostAsync(AccountRelativeUrl + "/register", register);
    }
}