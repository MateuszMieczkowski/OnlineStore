using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public record ChangeUserPassword(int Id, string CurrentPassword, string NewPassword, string ConfirmNewPassword) : ICommand;