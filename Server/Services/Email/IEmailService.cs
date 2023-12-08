using OnlineStore.Server.Emails.EmailDefinitions;

namespace OnlineStore.Server.Services.Email;

public interface IEmailService
{
    public Task SendEmailFromDefinitionAsync(EmailDefinition definition, CancellationToken cancellationToken = default);

    public Task SendEmailAsync(string recipientEmail,
        string recipientName,
        string htmlContent,
        string subject,
        CancellationToken cancellationToken = default);
}