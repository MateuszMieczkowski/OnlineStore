using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Clients;

public record ChangeUserPreferences(int UserId, UIThemeDto UiThemeDto, DisplayedPriceDto DisplayedPriceDto, bool IsSubscribedToNewsletter, int PageSize) : ICommand;
