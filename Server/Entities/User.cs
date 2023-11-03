﻿namespace OnlineStore.Server.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public ICollection<Order> Orders { get; set; } = default!;
}