using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace OnlineStore.Client.Providers;

public interface ICallContext
{
    Task<int> GetUserId();
    
    Task<bool> IsAuthenticated();

    Task<AuthenticationState> GetAuthenticationStateAsync();
}

public class ApiAuthenticationStateProvider : AuthenticationStateProvider, ICallContext
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly ILocalStorageService _localStorage;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<string> GetAuthenticationJwtToken()
    {
        return await _localStorage.GetItemAsync<string>("accessToken");
    }

    public async Task<bool> IsAuthenticated()
    {
        var authenticationState = await GetAuthenticationStateAsync();
        return authenticationState.User.Identity?.IsAuthenticated == true;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
        if (string.IsNullOrWhiteSpace(savedToken)) return new AuthenticationState(user);

        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);

        if (tokenContent.ValidTo < DateTime.Now) return new AuthenticationState(user);

        var claims = await GetClaims();

        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return new AuthenticationState(user);
    }

    public async Task LoggedIn()
    {
        var claims = await GetClaims();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task LoggedOut()
    {
        await _localStorage.RemoveItemAsync("accessToken");
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("accessToken");
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims.ToList();
        return claims;
    }

    public async Task<int> GetUserId()
    {
        var claims = await GetClaims();
        var userId = int.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return userId;
    }
}