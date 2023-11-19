using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Providers;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Services;

public interface IAccountService
{
    Task<bool> AuthenticateAsync(AuthenticateUser authenticateUser);
    
    Task RegisterUser(RegisterUser registerUser);
    
    Task Logout();
}

public class AccountService : IAccountService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IApiBroker _broker;
    private readonly ILocalStorageService _localStorage;

    public AccountService(IApiBroker broker, ILocalStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _broker = broker;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> AuthenticateAsync(AuthenticateUser authenticateUser)
    {
        var response = await _broker.LoginAsync(authenticateUser);

        await _localStorage.SetItemAsync("accessToken", response.Token);

        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
        return true;
    }

    public async Task RegisterUser(RegisterUser registerUser)
    {
        await _broker.RegisterAsync(registerUser);
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
    }
}