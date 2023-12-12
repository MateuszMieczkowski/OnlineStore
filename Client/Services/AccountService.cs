using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Models;
using OnlineStore.Client.Models.Accounts;
using OnlineStore.Client.Providers;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using System.Security.Claims;

namespace OnlineStore.Client.Services;

public interface IAccountService
{
    Task<bool> AuthenticateAsync(AuthenticateUser authenticateUser);
    
    Task RegisterAdmin(RegisterAdmin registerAdmin);
    
    Task<PagedResult<UserDto>> GetUserList(int pageNumber, int pageSize);
    
    Task Logout();
    
    Task ChangeUserPassword(ChangePasswordModel model);
    
    Task ForgotPassword(string email);

    Task ResetUserPassword(string token, ResetPasswordModel model);
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
        await _localStorage.SetItemAsync("email", response.Email);
        await _localStorage.SetItemAsync("preferences", response.Preferences);

        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
        return true;
    }

    public async Task RegisterAdmin(RegisterAdmin registerAdmin)
    {
        await _broker.RegisterAsync(registerAdmin);
    }

    public async Task<PagedResult<UserDto>> GetUserList(int pageNumber, int pageSize)
    {
        return await _broker.GetUserList(pageNumber, pageSize);
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        await _localStorage.RemoveItemsAsync(new []{LocalStorageKeys.UserPreferences});
    }

    public async Task ChangeUserPassword(ChangePasswordModel model)
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var userId = int.Parse(authenticationState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var command = new ChangeUserPassword(
            Id: userId,
            CurrentPassword: model.CurrentPassword,
            NewPassword: model.NewPassword,
            ConfirmNewPassword: model.ConfirmNewPassword);
        
        await _broker.ChangeUserPassword(command);
    }

    public async Task ForgotPassword(string email)
    {
        var command = new ForgotPassword(email);
        await _broker.ForgotUserPassword(command);
    }

    public async Task ResetUserPassword(string token, ResetPasswordModel model)
    {
        var command = new ResetPassword(token, model.NewPassword);
        await _broker.ResetUserPassword(command);
    }
}