using Microsoft.Extensions.Caching.Memory;

namespace OnlineStore.Server.Services.Email.EmailTemplateService;

public class CachedEmailTemplateService : IEmailTemplateService
{
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(60);
    private readonly DbEmailTemplateService _dbEmailTemplateService;

    private readonly IMemoryCache _memoryCache;


    public CachedEmailTemplateService(IMemoryCache memoryCache, DbEmailTemplateService dbEmailTemplateService)
    {
        _memoryCache = memoryCache;
        _dbEmailTemplateService = dbEmailTemplateService;
    }

    public async Task<string> GetEmailTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(templateName, out string? template)) return template!;

        template ??= await _dbEmailTemplateService.GetEmailTemplateAsync(templateName, cancellationToken);
        _memoryCache.Set(templateName, template, _cacheExpiration);
        return template;
    }

    public async Task AddOrUpdateEmailTemplateAsync(string templateName, string templateContent,
        CancellationToken cancellationToken = default)
    {
        await _dbEmailTemplateService.AddOrUpdateEmailTemplateAsync(templateName, templateContent, cancellationToken);
    }
}