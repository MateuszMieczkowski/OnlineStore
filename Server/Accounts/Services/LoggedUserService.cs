using System.Security.Claims;

namespace OnlineStore.Server.Accounts.Services;

public interface ILoggedUserService
{
    string? GetUserRole();
}
public class LoggedUserService : ILoggedUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public LoggedUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? GetUserRole()
    {
        var userClaims = _contextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();

        var role = userClaims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .FirstOrDefault();

        return role;
    }
}