namespace OnlineStore.Shared.Accounts;

/// <summary>
/// DTO containing login event information for a user
/// </summary>
public class LoginEventDto
{
    /// <summary>
    /// Last successful login timestamp
    /// </summary>
    public DateTime? LastSuccessfulLoginAt { get; set; }

    /// <summary>
    /// Last failed login attempt timestamp
    /// </summary>
    public DateTime? LastFailedLoginAt { get; set; }

    /// <summary>
    /// Number of failed login attempts since the last successful login
    /// </summary>
    public int FailedLoginAttemptsSinceLastSuccess { get; set; }
}

