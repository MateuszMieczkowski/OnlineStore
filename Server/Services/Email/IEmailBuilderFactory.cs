namespace OnlineStore.Server.Services.Email;

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
        return _serviceProvider.GetRequiredService<EmailBuilder>();
    }
}   