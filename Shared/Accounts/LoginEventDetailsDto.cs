namespace OnlineStore.Shared.Accounts;

/// <summary>
/// DTO containing login event history summary for a user
/// </summary>
public record LoginEventDetailsDto(
    List<LoginEventDetailDto> LoginEvents,
    DateTime? LastSuccessfulLoginAt,
    DateTime? LastFailedLoginAt);
