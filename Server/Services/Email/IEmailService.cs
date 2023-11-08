using OnlineStore.Server.Emails.TemplateDefinitions;

namespace OnlineStore.Server.Services.Email
{
    public interface IEmailService
    {
        public Task SendEmailFromTemplateAsync(EmailTemplateDefinition templateDefinition, CancellationToken cancellationToken = default);
    }
}
