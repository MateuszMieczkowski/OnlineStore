namespace OnlineStore.Server.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public decimal PriceNet { get; set; }
    public decimal PriceGross { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }

    public OrderItemProduct Product { get; set; } = default!;
}