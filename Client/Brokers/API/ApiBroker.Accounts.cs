using System.Net.Http.Json;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string AccountRelativeUrl = "api/account";

    public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync(AccountRelativeUrl + "/login", loginDto);

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();

        return auth;
    }

    public async Task RegisterAsync(RegisterUserDto registerDto)
    {
        await PostAsync(AccountRelativeUrl + "/register", registerDto);
    }
}