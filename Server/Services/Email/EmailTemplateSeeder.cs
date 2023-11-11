using OnlineStore.Server.Emails.TemplateDefinitions;
using OnlineStore.Server.Services.Email.EmailTemplateService;

namespace OnlineStore.Server.Services.Email
{
    public class EmailTemplateSeeder
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplateSeeder(IEmailTemplateServiceFactory factory)
        {
            _emailTemplateService = factory.Create();
        }

        public async Task SeedAsync()
        {
            await SeedTemplateAsync(new OrderCreatedSummaryEmail(null!, null!, null!, null!));
        }

        private async Task SeedTemplateAsync(EmailDefinition definition)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Emails", "Html", definition.TemplateName) + ".html";
            var htmlContext = await File.ReadAllTextAsync(path);
            await _emailTemplateService.AddOrUpdateEmailTemplateAsync(definition.TemplateName, htmlContext);
        }
    }
}
