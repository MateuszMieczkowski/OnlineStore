using OnlineStore.Server.Emails.EmailDefinitions;

namespace OnlineStore.Server.Services.Email.Builder;

public interface IEmailBuilder
{
    IEmailBuilder AddSubject(string subject);
    IEmailBuilder AddRecipientEmail(string email);
    IEmailBuilder AddRecipientName(string name);
    IEmailBuilder AddSenderEmail(string email);
    IEmailBuilder AddHtmlBody(string htmlBody);
    IEmailBuilder FromDefinition(EmailDefinition emailDefinition);
    Task<Entities.Email> BuildAsync(CancellationToken cancellationToken = default);
}