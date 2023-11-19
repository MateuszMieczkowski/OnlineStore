using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Repositories;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Handlers;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPassword>
{
    private readonly IAccountService _accountService;
    private readonly IUserRepository _userRepository;
    private readonly OnlineStoreDbContext _dbContext;

    public ResetPasswordCommandHandler(IAccountService accountService, IUserRepository userRepository, OnlineStoreDbContext dbContext)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Handle(ResetPassword command, CancellationToken cancellationToken)
    {
        var remindPasswordRequest = await _dbContext.RemindPasswordRequests.FirstOrDefaultAsync(x => x.Token == command.Token, cancellationToken);
        if (remindPasswordRequest is null)
        {
            return;
        }

        AssertRemindPasswordRequest(remindPasswordRequest);
        
        var user = await _userRepository.GetById(remindPasswordRequest.UserId);

        await _accountService.ChangePassword(user, command.Password);

        remindPasswordRequest.IsUsed = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void AssertRemindPasswordRequest(RemindPasswordRequest request)
    {
        if (request.IsUsed)
        {
            throw new BadRequestException("Token do zmiany hasła został już użyty.");
        }

        var isTokenExpired = request.CreatedTime.UtcDateTime.AddMinutes(30) < DateTimeOffset.UtcNow;
        if (isTokenExpired)
        {
            throw new BadRequestException("Token do zmiany hasła wygasł.");
        }
    }
}
