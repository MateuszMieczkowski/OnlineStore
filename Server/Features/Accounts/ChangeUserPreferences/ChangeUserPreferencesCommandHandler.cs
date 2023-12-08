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
        var userPreferences =
            await _dbContext.UserPreferences.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        var shouldCreatePreferences = userPreferences is null;

        if (shouldCreatePreferences)
        {
            userPreferences = new UserPreferences { UserId = userId };
        }

        userPreferences!.UITheme = (UITheme)command.UiThemeDto;
        userPreferences.DisplayedPrice = (DisplayedPrice)command.DisplayedPriceDto;
        userPreferences.IsSubscribedToNewsLetter = command.IsSubscribedToNewsletter;
        userPreferences.PageSize = command.PageSize;

        if (shouldCreatePreferences)
        {
            _dbContext.UserPreferences.Add(userPreferences);
        }
        else
        {
            _dbContext.UserPreferences.Update(userPreferences);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}