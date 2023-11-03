using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class Email
{
    public int Id { get; set; }
    public string SenderEmail { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string? RecipientName { get; set; }
    public string HtmlContent { get; set; } = string.Empty;
    public EmailStatus Status { get; set; }
    public int AttemptCount { get; set; }
}