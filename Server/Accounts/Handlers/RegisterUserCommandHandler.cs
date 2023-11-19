using OnlineStore.Server.Accounts.Strategies;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Handlers;

public class RegisterUserCommandHandler : ICommandHandler<RegisterAdmin>
{
    private readonly IAccountService _accountService;
    private readonly IUserFactory<RegisterAdmin> _factory;

    public RegisterUserCommandHandler(IAccountService accountService, IUserFactory<RegisterAdmin> factory)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task Handle(RegisterAdmin command, CancellationToken cancellationToken = default)
    {
        await _accountService.RegisterUser(command, _factory, cancellationToken);
    }
}
