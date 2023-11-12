using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Options;
using OnlineStore.Server.Services;
using Quartz;

namespace OnlineStore.Server.Jobs;

public class EmailSenderJob : IJob
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILogger<EmailSenderJob> _logger;
    private readonly IClock _clock; 
    private readonly SmtpOptions _smtpOptions;

    public EmailSenderJob
        (OnlineStoreDbContext dbContext,
        ILogger<EmailSenderJob> logger,
        IClock clock,
        IOptions<SmtpOptions> smtpOptions)
    {
        _dbContext = dbContext;
        _logger = logger;
        _clock = clock;
        _smtpOptions = smtpOptions.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("EmailSenderJob started");

        var smtpClient = CreateClient();

        var emailsToSend = await _dbContext.Emails
            .Where(e => e.Status == EmailStatus.ToSend)
            .ToListAsync(context.CancellationToken);

        foreach (var email in emailsToSend)
        {
            SendEmail(email, smtpClient);
        }
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("EmailSenderJob finished.");
    }

    private void SendEmail(Email email, SmtpClient smtpClient)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpOptions.Username),
            Subject = email.Subject,
            Body = email.HtmlContent,
            To = { email.RecipientEmail },
            IsBodyHtml = true,
        };
        email.AttemptCount++;

        try
        {
            smtpClient.Send(mailMessage);
            email.Status = EmailStatus.Sent;
            email.SentDate = _clock.UtcNow.UtcDateTime;
        }
        catch (Exception ex)
        {
            email.Status = email.AttemptCount >= 3 ? EmailStatus.Failed : EmailStatus.ToSend;

            _logger.LogError(ex, "Email with id: {emailId} failed to send", email.Id);
        }
    }

    private SmtpClient CreateClient()
    {
        
        var smtpClient = new SmtpClient(_smtpOptions.Host)
        {
            Port = _smtpOptions.Port,
            Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password),
            EnableSsl = _smtpOptions.EnableSsl,

        };
        return smtpClient;
    }
}