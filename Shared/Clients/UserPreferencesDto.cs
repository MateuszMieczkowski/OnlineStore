using OnlineStore.Shared.Enums;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Clients;

[DataContract]
public record UserPreferencesDto(UIThemeDto UiTheme, DisplayedPriceDto DisplayedPrice, bool IsSubscribedToNewsletter, int PageSize);
