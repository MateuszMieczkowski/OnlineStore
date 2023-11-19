namespace OnlineStore.Server.Entities;

public class RemindPasswordRequest
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    
    public DateTimeOffset CreatedTime { get; set; }
    
    public bool IsUsed { get; set; }
    public User User { get; set; } = default!;
}