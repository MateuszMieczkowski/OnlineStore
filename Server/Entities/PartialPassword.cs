namespace OnlineStore.Server.Entities;

public class PartialPassword
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Fragment { get; set; } = string.Empty; // The substring of the password
    public int StartPosition { get; set; } // 0-based start index in the full password
    public int Length { get; set; } // Length of the fragment

    public virtual User User { get; set; } = default!;
}
