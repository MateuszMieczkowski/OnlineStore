using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public record ForgotPassword(string Email) : ICommand;
