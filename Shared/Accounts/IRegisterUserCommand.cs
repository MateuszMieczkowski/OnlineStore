using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Accounts;

public interface IRegisterUserCommand : ICommand
{
    string Email { get; set; }

    string Password { get; set; }

    string ConfirmPassword { get; set; }
}
