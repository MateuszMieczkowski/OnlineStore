using OnlineStore.Server.Entities.Abstractions;

namespace OnlineStore.Server.Entities;

public class LoginEvent : ITimeCreated
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsSuccessful { get; set; }
    public string? FailureReason { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}

