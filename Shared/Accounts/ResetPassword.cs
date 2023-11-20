using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public record ResetPassword(string Token, string Password) : ICommand;
