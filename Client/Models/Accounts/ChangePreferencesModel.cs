using OnlineStore.Shared.Enums;

namespace OnlineStore.Client.Models.Accounts;

public class ChangePreferencesModel
{
    public DisplayedPriceDto DisplayedPrice { get; set; }

    public UIThemeDto UiTheme { get; set; }
    
    public bool IsSubscribedToNewsLetter { get; set; }
    
    public int PageSize { get; set; }
}