using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public class AuthenticateWithPartialPassword : IQuery<OnlineStore.Shared.Models.AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Fragment { get; set; } = string.Empty;
    public int StartPosition { get; set; }
    public int Length { get; set; }
    // What the user typed for each position in the fragment
    public string UserInput { get; set; } = string.Empty;
}
