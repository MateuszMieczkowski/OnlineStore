using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OnlineStore.Client.Models.Accounts;
using OnlineStore.Client.Providers;
using OnlineStore.Client.Services;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.SoapContracts;
using System.Security.Claims;

namespace OnlineStore.Client.SoapServices;

public class AccountSoapService(
    AuthenticationStateProvider authenticationStateProvider,
    ILocalStorageService localStorage,
    IShoppingCartService shoppingCartService,
    ISoapClient soapClient) 
    : IAccountService
{
    private const string Endpoint = "/soap/account";
    private const string ServiceNamespace = "http://tempuri.org/IAccountSoapService";

    public async Task<bool> AuthenticateAsync(AuthenticateUser authenticateUser)
    {
        var query = new AuthenticateUser
        {
            Email = authenticateUser.Email,
            Password = authenticateUser.Password
        };

        var response = await soapClient.SendQuery<AuthenticateUser, AuthResponse>(
            Endpoint, 
            ServiceNamespace,
            "Login",
            query);

        await localStorage.SetItemAsync("accessToken", response.Token);
        await localStorage.SetItemAsync("email", response.Email);
        // await localStorage.SetItemAsync("preferences", response.Preferences);
        await shoppingCartService.LoadCartFromServer();

        await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();
        return true;
    }

    public async Task RegisterAdmin(RegisterAdmin registerAdmin)
    {
        var command = new RegisterAdmin
        {
            Email = registerAdmin.Email, 
            Password = registerAdmin.Password,
            ConfirmPassword = registerAdmin.ConfirmPassword
        };
        
        await soapClient.SendCommand(
            Endpoint, 
            ServiceNamespace,
            "RegisterUser",
            command);
        // await accountSoapService.RegisterUserAsync(command);
    }

    public async Task<PagedResult<UserDto>> GetUserList(int pageNumber, int pageSize)
    {
        var result = await soapClient.SendQuery<GetUserList, UserListPagedResponseDto>(
            Endpoint, 
            ServiceNamespace,
            "GetUsers",
            new GetUserList(pageNumber, pageSize));
        
        return new PagedResult<UserDto>(
            items: result.Items,
            pageNumber: result.PageNumber,
            pageSize: result.PageSize,
            totalPages: result.TotalPages,
            totalItemsCount: result.TotalPages);
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        await localStorage.RemoveItemsAsync(new[] { LocalStorageKeys.UserPreferences });
        await shoppingCartService.SaveCartToServer();
        await shoppingCartService.ClearCart();
    }

    public async Task ChangeUserPassword(ChangePasswordModel model)
    {
        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var userId = int.Parse(authenticationState.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var command = new ChangeUserPassword(userId, model.CurrentPassword, model.NewPassword, model.ConfirmNewPassword);
        
        await soapClient.SendCommand(
            Endpoint, 
            ServiceNamespace,
            "ChangePassword",
            command);
        
        // await accountSoapService.ChangePasswordAsync(command);
    }

    public Task ForgotPassword(string email) => Task.CompletedTask;

    public Task ResetUserPassword(string token, ResetPasswordModel model)
        => Task.CompletedTask;
}
