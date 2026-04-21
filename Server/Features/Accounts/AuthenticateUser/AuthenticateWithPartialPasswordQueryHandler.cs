using Microsoft.AspNetCore.Http;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Features.Accounts.Repositories;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Features.Accounts.AuthenticateUser;

public class AuthenticateWithPartialPasswordQueryHandler(
    IAccountService accountService,
    ITokenGenerator tokenGenerator,
    IUserRepository userRepository,
    ILoginEventService loginEventService,
    IPartialPasswordChallengeStore challengeStore,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<AuthenticateWithPartialPassword, AuthResponse>
{
    public async Task<AuthResponse> Handle(AuthenticateWithPartialPassword request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUserByEmail(request.Email);

        try
        {
            if (user == null)
                throw new InvalidCredentialsException();

            var challenge = challengeStore.ConsumeChallenge(request.ChallengeToken);
            if (challenge == null || challenge.Value.UserId != user.Id)
                throw new InvalidCredentialsException();

            var isValid = await accountService.VerifyPartialPassword(user.Id, challenge.Value.PartialPasswordId, request.UserInput);
            if (!isValid)
                throw new InvalidCredentialsException();
        }
        catch
        {
            var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            await loginEventService.RecordLoginEventAsync(user?.Id ?? 1, false, "Invalid partial password", ipAddress, request.Email, cancellationToken);

            user = await userRepository.FindUserByEmail(request.Email);
            int failedAttempts = user?.FailedLoginAttemptsSinceLastSuccess ?? 0;
            int delaySeconds = (int)Math.Min(Math.Pow(2, failedAttempts), 60);
            if (failedAttempts > 0 && delaySeconds > 0)
                throw new TooManyFailedLoginAttemptsException(delaySeconds);
            throw;
        }

        var successIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var successAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
        await loginEventService.RecordLoginEventAsync(user.Id, true, null, successIp, successAgent, cancellationToken);

        user = await userRepository.FindUserByEmail(request.Email);

        var preferences = user!.Preferences;
        var prefs = preferences == null
            ? new UserPreferencesDto(UiTheme: UIThemeDto.Light, DisplayedPrice: DisplayedPriceDto.Gross, IsSubscribedToNewsletter: false, PageSize: 20, IsPasswordManagerEnabled: true)
            : new UserPreferencesDto(
                UiTheme: (UIThemeDto)preferences.UITheme,
                DisplayedPrice: (DisplayedPriceDto)preferences.DisplayedPrice,
                IsSubscribedToNewsletter: preferences.IsSubscribedToNewsLetter,
                PageSize: preferences.PageSize,
                IsPasswordManagerEnabled: preferences.IsPasswordManagerEnabled);

        return new AuthResponse
        {
            Id = user.Id,
            Email = user.Email,
            Preferences = prefs,
            Token = tokenGenerator.GenerateJwtToken(user),
            LastSuccessfulLoginAt = user.LastSuccessfulLoginAt,
            LastFailedLoginAt = user.LastFailedLoginAt,
            FailedLoginAttemptsSinceLastSuccess = user.FailedLoginAttemptsSinceLastSuccess
        };
    }
}
