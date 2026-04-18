﻿using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public UserRole UserRole { get; set; }

    // Login event tracking
    public DateTime? LastSuccessfulLoginAt { get; set; }
    public DateTime? LastFailedLoginAt { get; set; }
    public int FailedLoginAttemptsSinceLastSuccess { get; set; }

    public virtual string FullName => Email;

    public ICollection<Order> Orders { get; set; } = default!;

    public UserPreferences? Preferences { get; set; }
    
    public virtual ICollection<LoginEvent> LoginEvents { get; set; } = new List<LoginEvent>();

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void RecordSuccessfulLogin()
    {
        LastSuccessfulLoginAt = DateTime.UtcNow;
        FailedLoginAttemptsSinceLastSuccess = 0;
    }

    public void RecordFailedLogin()
    {
        LastFailedLoginAt = DateTime.UtcNow;
        FailedLoginAttemptsSinceLastSuccess++;
    }
}