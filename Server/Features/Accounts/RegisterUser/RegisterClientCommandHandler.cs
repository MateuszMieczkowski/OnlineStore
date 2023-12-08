using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Features.Accounts.Strategies;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Clients;

namespace OnlineStore.Server.Features.Accounts.RegisterUser;

public class RegisterClientCommandHandler : ICommandHandler<RegisterClient>
{
    private readonly IAccountService _accountService;
    private readonly IUserFactory<RegisterClient> _factory;

    public RegisterClientCommandHandler(IAccountService accountService, IUserFactory<RegisterClient> factory)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task Handle(RegisterClient command, CancellationToken cancellationToken = default)
    {
        await _accountService.RegisterUser(command, _factory, cancellationToken);
    }
}
