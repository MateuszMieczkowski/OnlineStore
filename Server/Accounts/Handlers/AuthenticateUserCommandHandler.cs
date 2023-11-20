using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Repositories;
using OnlineStore.Server.Accounts.Services;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Accounts.Handlers;

public class AuthenticateUserQueryHandler : IQueryHandler<AuthenticateUser, AuthResponse>
{
    private readonly IAccountService _accountService;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly OnlineStoreDbContext _context;

    public AuthenticateUserQueryHandler(
        IAccountService accountService,
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        OnlineStoreDbContext context)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _context = context;
    }

    public async Task<AuthResponse> Handle(AuthenticateUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmail(request.Email);
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        _accountService.AssertHashedPassword(user, request.Password); 

        var authResponse = new AuthResponse
        {
            Id = user.Id,
            Email = user.Email,
            Preferences = await GetClientPreferences(user),
            Token = _tokenGenerator.GenerateJwtToken(user)
        };

        return authResponse;
    }

    private async Task<UserPreferencesDto> GetClientPreferences(User user)
    {
        var preferences = await _context.UserPreferences.FirstOrDefaultAsync(x => x.UserId == user.Id);
        
        var defaultUiTheme = UITheme.Light;
        var defaultDisplayPrice = DisplayedPrice.Gross;
        var defaultIsSubscribedToNewsletter = false;

        if (preferences == null)
        {
            return new UserPreferencesDto(
                UiTheme: (UIThemeDto)defaultUiTheme,
                DisplayedPrice: (DisplayedPriceDto)defaultDisplayPrice,
                IsSubscribedToNewsletter: defaultIsSubscribedToNewsletter);
        }

        return new UserPreferencesDto(
            UiTheme:  (UIThemeDto)preferences.UITheme,
            DisplayedPrice: (DisplayedPriceDto)preferences.DisplayedPrice,
            IsSubscribedToNewsletter: defaultIsSubscribedToNewsletter); // TODO move IsSubscribedToNewsletter to UserPreferences entity
    }
}
