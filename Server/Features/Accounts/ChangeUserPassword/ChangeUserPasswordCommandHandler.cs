using OnlineStore.Server.Features.Accounts.Repositories;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Features.Accounts.ChangeUserPassword;

public class ChangeUserPasswordCommandHandler : ICommandHandler<Shared.Accounts.ChangeUserPassword>
{
    private readonly IAccountService _accountService;
    private readonly IUserRepository _userRepository;

    public ChangeUserPasswordCommandHandler(IAccountService accountService, IUserRepository userRepository)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }


    public async Task Handle(Shared.Accounts.ChangeUserPassword command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id)
            ?? throw new NotFoundException($"Nie znaleziono użytkownika o ID {command.Id}");

        _accountService.AssertHashedPassword(user, command.CurrentPassword);

        await _accountService.ChangePassword(user, command.CurrentPassword);
    }
}
