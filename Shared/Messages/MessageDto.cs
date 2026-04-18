namespace OnlineStore.Shared.Messages;

public class MessageDto
{
    public Guid Id { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string[] AllowedEditors { get; set; }
}

