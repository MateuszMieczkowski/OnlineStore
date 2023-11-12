namespace OnlineStore.Server.Services.Email.EmailTemplateService;

public interface IEmailTemplateService
{
    Task<string> GetEmailTemplateAsync(string templateName, CancellationToken cancellationToken = default);

    Task AddOrUpdateEmailTemplateAsync(string templateName, string templateContent,
        CancellationToken cancellationToken = default);
}