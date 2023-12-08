using OnlineStore.Server.Services.Email.EmailTemplateService;

namespace OnlineStore.Server.Services.Email.Builder;

public interface IEmailBuilderFactory
{
    IEmailBuilder Create();
}

public class EmailBuilderFactory : IEmailBuilderFactory
{
    private readonly IEmailTemplateServiceFactory _emailTemplateServiceFactory;

    public EmailBuilderFactory(IEmailTemplateServiceFactory serviceProvider)
    {
        _emailTemplateServiceFactory = serviceProvider;
    }

    public IEmailBuilder Create()
    {
        return new EmailBuilder(_emailTemplateServiceFactory);
    }
}