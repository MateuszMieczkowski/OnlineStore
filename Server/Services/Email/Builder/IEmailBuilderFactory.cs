using OnlineStore.Server.Services.Email.EmailTemplateService;

namespace OnlineStore.Server.Services.Email.Builder;

public interface IEmailBuilderFactory
{
    IEmailBuilder Create();
}

public class EmailBuilderFactory : IEmailBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmailBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEmailBuilder Create()
    {
        var emailTemplateServiceFactory = _serviceProvider.GetRequiredService<IEmailTemplateServiceFactory>();
        return new EmailBuilder(emailTemplateServiceFactory);
    }
}