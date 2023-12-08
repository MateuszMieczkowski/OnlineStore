using Microsoft.Extensions.Caching.Memory;

namespace OnlineStore.Server.Services.Email.EmailTemplateService;

public interface IEmailTemplateServiceFactory
{
    IEmailTemplateService Create();
}

public class EmailTemplateServiceFactory : IEmailTemplateServiceFactory
{

    private readonly OnlineStoreDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
	public EmailTemplateServiceFactory(OnlineStoreDbContext dbContext, IMemoryCache memoryCache)
	{
		_dbContext = dbContext;
		_memoryCache = memoryCache;
	}

	public IEmailTemplateService Create()
    {
        var dbEmailTemplateService = new DbEmailTemplateService(_dbContext);

        return new CachedEmailTemplateService(_memoryCache, dbEmailTemplateService);
    }
}