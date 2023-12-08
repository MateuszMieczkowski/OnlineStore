using System.Security.Claims;

namespace OnlineStore.Server.Features.Accounts.Services;

public interface ILoggedUserService
{
    string? GetUserRole();
    int? GetUserId();
}

public class LoggedUserService : ILoggedUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public LoggedUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? GetUserRole() => GetClaim(ClaimTypes.Role);

    public int? GetUserId()
    {
        var userIdString = GetClaim(ClaimTypes.NameIdentifier);
        var userId = userIdString != null ? int.Parse(userIdString) : (int?)null;        
        return userId;
    }
    
    private string? GetClaim(string type)
    {
        var claims = _contextAccessor.HttpContext?.User.Claims ?? Enumerable.Empty<Claim>();
        var claim = claims
            .Where(x => x.Type == type)
            .Select(x => x.Value)
            .FirstOrDefault();

        return claim;
    }
}