using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Repositories;
using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Email;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Accounts.Handlers;

public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPassword>
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly OnlineStoreDbContext _dbContext;

    public ForgotPasswordCommandHandler(
        IConfiguration configuration,
        IEmailService emailService,
        IUserRepository userRepository,
        OnlineStoreDbContext dbContext)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    }

    public async Task Handle(ForgotPassword command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindUserByEmail(command.Email);
        if (user is null) 
        {
            return; // safety reasons
        }

        var remindPasswordRequest = new RemindPasswordRequest
        {
            UserId = user.Id,
            User = user,
            CreatedTime = DateTimeOffset.UtcNow,
            Token = Guid.NewGuid().ToString(),
            IsUsed = false,
        };

        await ScheduleSendResetPasswordEmail(user.Email, remindPasswordRequest);
        
        _dbContext.RemindPasswordRequests.Add(remindPasswordRequest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ScheduleSendResetPasswordEmail(string email, RemindPasswordRequest request)
    {
        var frontendUrl = _configuration.GetValue<string>("ClientAddress");
        var resetLink = $"{frontendUrl}/users/reset-password/{request.Token}";

        var emailDefinition = new ResetPasswordEmail(
            resetLink,
            recipientEmail: email,
            recipientName: null,
            senderEmail: null);

        await _emailService.SendEmailFromDefinitionAsync(emailDefinition);
    }
}
