using OnlineStore.Shared.Enums;

namespace OnlineStore.Client.Models;

public class ChangePreferencesModel
{
    public DisplayedPriceDto DisplayedPrice { get; set; }

    public UIThemeDto UiTheme { get; set; }
}