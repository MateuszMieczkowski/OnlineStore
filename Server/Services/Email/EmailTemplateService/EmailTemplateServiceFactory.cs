using Microsoft.Extensions.Caching.Memory;

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
        var dbContext = _serviceProvider.GetRequiredService<OnlineStoreDbContext>();
        var memoryCache = _serviceProvider.GetRequiredService<IMemoryCache>();
        var dbEmailTemplateService = new DbEmailTemplateService(dbContext);

        return new CachedEmailTemplateService(memoryCache, dbEmailTemplateService);
    }
}