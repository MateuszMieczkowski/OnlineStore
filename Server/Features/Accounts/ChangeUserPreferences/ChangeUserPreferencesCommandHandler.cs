using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Accounts.ChangeUserPreferences;

public class ChangeUserPreferencesCommandHandler : ICommandHandler<Shared.Clients.ChangeUserPreferences>
{
    private readonly OnlineStoreDbContext _dbContext;

    public ChangeUserPreferencesCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    public async Task Handle(Shared.Clients.ChangeUserPreferences command, CancellationToken cancellationToken)
    {
        var userId = command.UserId;
        var user = await _dbContext.Users.SingleAsync(x => x.Id == userId, cancellationToken);
        user.Preferences ??= new UserPreferences { UserId = userId };

        user.Preferences.UITheme = (UITheme)command.UiThemeDto;
        user.Preferences.DisplayedPrice = (DisplayedPrice)command.DisplayedPriceDto;
        user.Preferences.IsSubscribedToNewsLetter = command.IsSubscribedToNewsletter;
        user.Preferences.PageSize = command.PageSize;
        user.Preferences.IsPasswordManagerEnabled = command.IsPasswordManagerEnabled;

        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}