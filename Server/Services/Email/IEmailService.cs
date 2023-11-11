using OnlineStore.Server.Emails.EmailDefinitions;

namespace OnlineStore.Server.Services.Email;

public interface IEmailService
{
    public Task SendEmailFromTemplateAsync(EmailDefinition definition, CancellationToken cancellationToken = default);
}