using OnlineStore.Server.Emails.EmailDefinitions;

namespace OnlineStore.Server.Services.Email;

public interface IEmailService
{
    public Task SendEmailFromDefinitionAsync(EmailDefinition definition, CancellationToken cancellationToken = default);
}