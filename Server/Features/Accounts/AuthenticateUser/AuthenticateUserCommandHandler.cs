using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Features.Accounts.Repositories;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Features.Accounts.AuthenticateUser;

public class AuthenticateUserQueryHandler(
    IAccountService accountService,
    ITokenGenerator tokenGenerator,
    IUserRepository userRepository,
    ILoginEventService loginEventService,
    OnlineStoreDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<Shared.Models.AuthenticateUser, AuthResponse>
{
    private readonly IAccountService _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly ILoginEventService _loginEventService = loginEventService ?? throw new ArgumentNullException(nameof(loginEventService));
    private readonly OnlineStoreDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public async Task<AuthResponse> Handle(Shared.Models.AuthenticateUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmail(request.Email);
        try
        {
        
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }
            _accountService.AssertHashedPassword(user, request.Password);
        }
        catch
        {
            // Record failed login attempt
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Email;
            await _loginEventService.RecordLoginEventAsync(user?.Id ?? 1, false, "Invalid password", ipAddress, userAgent, cancellationToken);

            // Refresh user data to get updated failed attempts
            user = await _userRepository.FindUserByEmail(request.Email);
            int failedAttempts = user?.FailedLoginAttemptsSinceLastSuccess ?? 0;
            int delaySeconds = CalculateDelaySeconds(failedAttempts);
            if (failedAttempts > 0 && delaySeconds > 0)
            {
                throw new TooManyFailedLoginAttemptsException(delaySeconds);
            }
            throw;
        }

        // Record successful login
        var successIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var successUserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
        await _loginEventService.RecordLoginEventAsync(user.Id, true, null, successIpAddress, successUserAgent, cancellationToken);

        // Refresh user data to get updated login tracking info
        user = await _userRepository.FindUserByEmail(request.Email);

        var authResponse = new AuthResponse
        {
            Id = user.Id,
            Email = user.Email,
            Preferences = GetClientPreferences(user),
            Token = _tokenGenerator.GenerateJwtToken(user),
            LastSuccessfulLoginAt = user.LastSuccessfulLoginAt,
            LastFailedLoginAt = user.LastFailedLoginAt,
            FailedLoginAttemptsSinceLastSuccess = user.FailedLoginAttemptsSinceLastSuccess
        };

        return authResponse;
    }

    private int CalculateDelaySeconds(int failedAttempts)
    {
        // Exponential backoff: 2^attempts seconds, max 60s
        if (failedAttempts <= 0) return 0;
        int delay = (int)Math.Min(Math.Pow(2, failedAttempts), 60);
        return delay;
    }

    private UserPreferencesDto GetClientPreferences(User user)
    {
        var preferences = user.Preferences;
        
        var defaultUiTheme = UITheme.Light;
        var defaultDisplayPrice = DisplayedPrice.Gross;
        var defaultIsSubscribedToNewsletter = false;
        var defaultPageSize = 20;
        var defaultIsPasswordManagerEnabled = true;

        if (preferences == null)
        {
            return new UserPreferencesDto(
                UiTheme: (UIThemeDto)defaultUiTheme,
                DisplayedPrice: (DisplayedPriceDto)defaultDisplayPrice,
                IsSubscribedToNewsletter: defaultIsSubscribedToNewsletter,
                PageSize: defaultPageSize,
                IsPasswordManagerEnabled: defaultIsPasswordManagerEnabled);
        }

        return new UserPreferencesDto(
            UiTheme:  (UIThemeDto)preferences.UITheme,
            DisplayedPrice: (DisplayedPriceDto)preferences.DisplayedPrice,
            IsSubscribedToNewsletter: preferences.IsSubscribedToNewsLetter,
            PageSize: preferences.PageSize,
            IsPasswordManagerEnabled: preferences.IsPasswordManagerEnabled);
    }
}
