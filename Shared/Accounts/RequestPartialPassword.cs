using MediatR;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public class RequestPartialPassword : IQuery<PartialPasswordResponse>
{
    public string Email { get; set; } = string.Empty;
}

public class PartialPasswordResponse
{
    /// <summary>
    /// 1-based start position (for display to user)
    /// </summary>
    public int StartPosition { get; set; }
    public int Length { get; set; }
    /// <summary>
    /// An opaque token that the server uses to identify which partial password challenge was issued.
    /// </summary>
    public string ChallengeToken { get; set; } = string.Empty;
}
