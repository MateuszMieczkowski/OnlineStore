using OnlineStore.Server.Entities;
using OnlineStore.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Server.Features.Accounts.Services;

public interface ILoginEventService
{
    Task RecordLoginEventAsync(int userId, bool isSuccessful, string? failureReason = null, 
        string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);
    
    Task<LoginEventDto?> GetLastSuccessfulLoginAsync(int userId, CancellationToken cancellationToken = default);
    
    Task<LoginEventDto?> GetLastFailedLoginAsync(int userId, CancellationToken cancellationToken = default);
    
    Task<int> GetFailedLoginCountSinceLastSuccessAsync(int userId, CancellationToken cancellationToken = default);
    
    Task<List<LoginEventDetailDto>> GetLoginHistoryAsync(int userId, int limit = 10, CancellationToken cancellationToken = default);
}

public class LoginEventService : ILoginEventService
{
    private readonly OnlineStoreDbContext _context;
    private readonly IClock _clock;

    public LoginEventService(OnlineStoreDbContext context, IClock clock)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public async Task RecordLoginEventAsync(int userId, bool isSuccessful, string? failureReason = null, 
        string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var loginEvent = new LoginEvent
        {
            UserId = userId,
            IsSuccessful = isSuccessful,
            FailureReason = failureReason,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedDate = _clock.UtcNow.UtcDateTime
        };

        _context.LoginEvents.Add(loginEvent);

        var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);
        if (user != null)
        {
            if (isSuccessful)
            {
                user.RecordSuccessfulLogin();
            }
            else
            {
                user.RecordFailedLogin();
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoginEventDto?> GetLastSuccessfulLoginAsync(int userId, CancellationToken cancellationToken = default)
    {
        var loginEvent = await _context.LoginEvents
            .Where(x => x.UserId == userId && x.IsSuccessful)
            .OrderByDescending(x => x.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);

        return loginEvent == null ? null : new LoginEventDto(loginEvent.CreatedDate, true, loginEvent.IpAddress);
    }

    public async Task<LoginEventDto?> GetLastFailedLoginAsync(int userId, CancellationToken cancellationToken = default)
    {
        var loginEvent = await _context.LoginEvents
            .Where(x => x.UserId == userId && !x.IsSuccessful)
            .OrderByDescending(x => x.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);

        return loginEvent == null ? null : new LoginEventDto(loginEvent.CreatedDate, false, loginEvent.FailureReason);
    }

    public async Task<int> GetFailedLoginCountSinceLastSuccessAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        return user?.FailedLoginAttemptsSinceLastSuccess ?? 0;
    }

    public async Task<List<LoginEventDetailDto>> GetLoginHistoryAsync(int userId, int limit = 10, CancellationToken cancellationToken = default)
    {
        var events = await _context.LoginEvents
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedDate)
            .Take(limit)
            .Select(x => new LoginEventDetailDto(
                x.Id,
                x.CreatedDate,
                x.IsSuccessful,
                x.FailureReason,
                x.IpAddress,
                x.UserAgent))
            .ToListAsync(cancellationToken);

        return events;
    }
}

public record LoginEventDto(DateTime EventDate, bool IsSuccessful, string? AdditionalInfo);

public record LoginEventDetailDto(int Id, DateTime EventDate, bool IsSuccessful, string? FailureReason, string? IpAddress, string? UserAgent);



