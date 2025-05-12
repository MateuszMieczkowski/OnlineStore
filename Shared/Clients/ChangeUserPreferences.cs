using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Infrastructure;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Clients;

public record ChangeUserPreferences(
    int UserId,
    UIThemeDto UiThemeDto,
    DisplayedPriceDto DisplayedPriceDto,
    bool IsSubscribedToNewsletter,
    int PageSize) : ICommand;
