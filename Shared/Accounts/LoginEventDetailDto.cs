namespace OnlineStore.Shared.Accounts;

/// <summary>
/// DTO containing detailed login event information for retrieving event history
/// </summary>
public record LoginEventDetailDto(
    int Id,
    DateTime EventDate,
    bool IsSuccessful,
    string? FailureReason,
    string? IpAddress,
    string? UserAgent);