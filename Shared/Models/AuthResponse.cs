using OnlineStore.Shared.Clients;

namespace OnlineStore.Shared.Models;

public class AuthResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    
    public UserPreferencesDto? Preferences { get; set; }
}