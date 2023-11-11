namespace OnlineStore.Server.Emails.EmailDefinitions;

public abstract class EmailDefinition
{
    protected EmailDefinition(string recipientEmail, string? recipientName, string? senderEmail)
    {
        RecipientEmail = recipientEmail;
        RecipientName = recipientName;
        SenderEmail = senderEmail;
    }

    public string? SenderEmail { get; private set; }
    public string RecipientEmail { get; private set; }
    public string? RecipientName { get; private set; }

    public abstract string Subject { get; }
    public abstract string TemplateName { get; }
    public abstract ICollection<EmailReplacement> GetReplacements();
}

public record EmailReplacement(string Key, string Value);