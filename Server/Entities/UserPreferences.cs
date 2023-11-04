using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class UserPreferences
{
    public int Id { get; set; }
    public UITheme UITheme { get; set; }
    public DisplayedPrice DisplayedPrice { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = default!;
}