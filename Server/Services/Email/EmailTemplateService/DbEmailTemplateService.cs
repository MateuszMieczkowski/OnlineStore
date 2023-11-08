using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Services.Email.EmailTemplateService;

public class DbEmailTemplateService : IEmailTemplateService
{
    private readonly OnlineStoreDbContext _dbContext;

    public DbEmailTemplateService(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GetEmailTemplateAsync(string templateName, CancellationToken cancellationToken = default)
    {
        var template = await _dbContext.EmailTemplates
            .AsNoTracking()
            .Where(e => e.Name == templateName)
            .Select(e => e.HtmlContent)
            .FirstOrDefaultAsync(cancellationToken) ??
                throw new NotFoundException($"Email template with name {templateName} not found");

        return template;
    }

    public async Task AddEmailTemplateAsync(string templateName, string templateContent,
        CancellationToken cancellationToken = default)
    {
        _dbContext.EmailTemplates.Add(new EmailTemplate
        {
            Name = templateName,
            HtmlContent = templateContent
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}