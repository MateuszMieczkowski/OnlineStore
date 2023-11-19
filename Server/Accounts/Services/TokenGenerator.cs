using Microsoft.IdentityModel.Tokens;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Server.Accounts.Services;

public interface ITokenGenerator
{
    string GenerateJwtToken(User user);
}

public class TokenGenerator : ITokenGenerator
{
    private readonly AuthenticationSettings _authenticationSettings;
    
    public TokenGenerator(AuthenticationSettings authenticationSettings)
    {
        _authenticationSettings = authenticationSettings ?? throw new ArgumentNullException(nameof(authenticationSettings));
    }
    
    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.UserRole.ToString()),
        };

        return CreateToken(claims);
    }

    private string CreateToken(List<Claim> claims)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        var bearerToken = tokenHandler.WriteToken(token);
        return bearerToken;
    }

}
