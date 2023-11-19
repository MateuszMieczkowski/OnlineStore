using OnlineStore.Server.Accounts.Repositories;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Handlers;

public class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPassword>
{
    private readonly IAccountService _accountService;
    private readonly IUserRepository _userRepository;

    public ChangeUserPasswordCommandHandler(IAccountService accountService, IUserRepository userRepository)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }


    public async Task Handle(ChangeUserPassword command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(command.Id);

        _accountService.AssertHashedPassword(user, command.CurrentPassword);

        await _accountService.ChangePassword(user, command.CurrentPassword);
    }
}
