using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Services.Email.Builder;

namespace OnlineStore.Server.Services.Email;

public class EmailService : IEmailService
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly IEmailBuilderFactory _emailBuilderFactory;

    public EmailService(OnlineStoreDbContext dbContext, IEmailBuilderFactory emailBuilderFactory)
    {
        _dbContext = dbContext;
        _emailBuilderFactory = emailBuilderFactory;
    }

    public async Task SendEmailFromDefinitionAsync(EmailDefinition definition,
        CancellationToken cancellationToken = default)
    {
        var builder = _emailBuilderFactory.Create();

        var email = await builder.FromDefinition(definition)
            .BuildAsync(cancellationToken);

        _dbContext.Emails.Add(email);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SendEmailAsync(
        string recipientEmail,
        string recipientName,
        string htmlContent,
        string subject,
        CancellationToken cancellationToken)
    {
        var builder = _emailBuilderFactory.Create();
        var email = await builder.AddSubject(subject)
            .AddRecipientName(recipientName)
            .AddRecipientEmail(recipientEmail)
            .AddHtmlBody(htmlContent)
            .BuildAsync(cancellationToken);
        
        _dbContext.Emails.Add(email);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
    }
}