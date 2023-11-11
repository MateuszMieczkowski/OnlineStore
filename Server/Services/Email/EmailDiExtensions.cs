using OnlineStore.Server.Services.Email.Builder;
using OnlineStore.Server.Services.Email.EmailTemplateService;

namespace OnlineStore.Server.Services.Email;

public static class EmailDiExtensions
{
    public static IServiceCollection RegisterEmailServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailTemplateServiceFactory, EmailTemplateServiceFactory>();
        services.AddScoped<IEmailBuilderFactory, EmailBuilderFactory>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<EmailTemplateSeeder>();

        return services;
    }
}