using OnlineStore.Shared.Enums;

namespace OnlineStore.Shared.Clients;

public record UserPreferencesDto(UIThemeDto UiTheme, DisplayedPriceDto DisplayedPrice, bool IsSubscribedToNewsletter, int PageSize);
