namespace OnlineStore.Server.Entities;

public class Client : User
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public override string FullName => $"{FirstName} {LastName}";
    
    public ICollection<Order> Orders { get; set; } = default!;
}