using OnlineStore.Server.Emails.TemplateDefinitions;
using OnlineStore.Server.Services.Email.EmailTemplateService;

namespace OnlineStore.Server.Services.Email;

public class EmailBuilder : IEmailBuilder
{
    private readonly IEmailTemplateService _emailTemplateService;

    private Entities.Email _email = new();
    private EmailTemplateDefinition? _templateDefinition;

    public EmailBuilder(IEmailTemplateServiceFactory emailTemplateServiceFactory)
    {
        _emailTemplateService = emailTemplateServiceFactory.Create();
    }

    public IEmailBuilder AddSubject(string subject)
    {
       _email.Subject = subject;
       return this;
    }

    public IEmailBuilder AddRecipientEmail(string email)
    {
        _email.RecipientEmail = email;
        return this;
    }

    public IEmailBuilder AddRecipientName(string? name)
    {
        _email.RecipientName = name;
        return this;
    }

    public IEmailBuilder AddSenderEmail(string email)
    {
        _email.SenderEmail = email;
        return this;
    }

    public IEmailBuilder AddHtmlBody(string htmlBody)
    {
        _email.HtmlContent = htmlBody;
        return this;
    }


    public IEmailBuilder FromTemplate(EmailTemplateDefinition emailTemplateDefinition)
    {
        _templateDefinition = emailTemplateDefinition;
        return this;
    }

    public async Task<Entities.Email> BuildAsync(CancellationToken cancellationToken = default)
    {
        if (_templateDefinition is not null)
        {
            return await BuildFromTemplateAsync(cancellationToken);
        }

        return _email;
    }

    private async Task<Entities.Email> BuildFromTemplateAsync(CancellationToken cancellationToken)
    {
        _email = new Entities.Email()
        {
            Subject = _templateDefinition!.Subject,
            RecipientEmail = _templateDefinition.RecipientEmail,
            RecipientName = _templateDefinition.RecipientName,
            SenderEmail = _templateDefinition.SenderEmail
        };

        var template = await _emailTemplateService.GetEmailTemplateAsync(_templateDefinition!.TemplateName, cancellationToken);
        var replacements = _templateDefinition.GetReplacements();

        foreach (var (key, value) in replacements)
        {
            template = template.Replace(key, value);
        }
        _email.HtmlContent = template;

        return _email;
    }
}