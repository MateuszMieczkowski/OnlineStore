using MediatR;
using OnlineStore.Server.Services;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUser>
{
    private readonly IAccountService _accountService;
    
    public RegisterUserCommandHandler(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    public async Task Handle(RegisterUser command, CancellationToken cancellationToken = default)
    {
        await _accountService.RegisterUser(command, cancellationToken);
    }
}
