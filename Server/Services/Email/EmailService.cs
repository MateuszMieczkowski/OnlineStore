using OnlineStore.Server.Emails.TemplateDefinitions;
using OnlineStore.Server.Services.Email.Builder;

namespace OnlineStore.Server.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly OnlineStoreDbContext _dbContext;
        private readonly IEmailBuilderFactory _emailBuilderFactory;

        public EmailService(OnlineStoreDbContext dbContext, IEmailBuilderFactory emailBuilderFactory)
        {
            _dbContext = dbContext;
            _emailBuilderFactory = emailBuilderFactory;
        }

        public async Task SendEmailFromTemplateAsync(EmailDefinition definition,
            CancellationToken cancellationToken = default)
        {
            var builder = _emailBuilderFactory.Create();

            var email = await builder.FromTemplate(definition)
                .BuildAsync(cancellationToken);

            _dbContext.Emails.Add(email);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
