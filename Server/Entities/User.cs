using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public UserRole UserRole { get; set; }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}