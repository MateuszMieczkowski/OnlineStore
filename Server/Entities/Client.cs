namespace OnlineStore.Server.Entities
{
    public class Client : User
    {
        public ICollection<Order> Orders { get; set; } = default!;
    }
}
