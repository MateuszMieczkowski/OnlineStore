using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public class AuthenticateWithPartialPassword : IQuery<OnlineStore.Shared.Models.AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// The opaque challenge token returned by the server when requesting partial password.
    /// </summary>
    public string ChallengeToken { get; set; } = string.Empty;
    /// <summary>
    /// The user's input for the requested fragment positions.
    /// </summary>
    public string UserInput { get; set; } = string.Empty;
}
