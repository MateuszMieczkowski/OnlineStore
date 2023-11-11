using OnlineStore.Server.Emails.TemplateDefinitions;

namespace OnlineStore.Server.Services.Email.Builder;

public interface IEmailBuilder
{
    IEmailBuilder AddSubject(string subject);
    IEmailBuilder AddRecipientEmail(string email);
    IEmailBuilder AddRecipientName(string name);
    IEmailBuilder AddSenderEmail(string email);
    IEmailBuilder AddHtmlBody(string htmlBody);
    IEmailBuilder FromTemplate(EmailDefinition emailDefinition);
    Task<Entities.Email> BuildAsync(CancellationToken cancellationToken = default);
}