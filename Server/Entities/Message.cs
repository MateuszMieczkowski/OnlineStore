using OnlineStore.Server.Entities.Abstractions;

namespace OnlineStore.Server.Entities;

public class Message : ITimeCreated, ITimeModified
{
    public Guid Id { get; set; }
    public int AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public User? Author { get; set; }
    public ICollection<MessagePermission> AllowedEditors { get; set; } = new List<MessagePermission>();
}

