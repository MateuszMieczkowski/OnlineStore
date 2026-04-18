namespace OnlineStore.Server.Entities;

public class MessagePermission
{
    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public int UserId { get; set; }

    public Message? Message { get; set; }
    
    public User? User { get; set; }
}

