namespace OnlineStore.Server.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal PriceNet { get; set; }
    public decimal PriceGross { get; set; }
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;
    public Order Order { get; set; } = default!;
}