namespace OnlineStore.Server.Services.Email.EmailTemplateService;

public interface IEmailTemplateServiceFactory
{
    IEmailTemplateService Create();
}


public class EmailTemplateServiceFactory : IEmailTemplateServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmailTemplateServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEmailTemplateService Create()
    {
        return _serviceProvider.GetRequiredService<CachedEmailTemplateService>();
    }
}