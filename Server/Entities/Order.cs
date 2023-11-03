using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class Order
{
    public int Id { get; set; }
    public decimal TotalNet { get; set; }
    public decimal TotalGross { get; set; }
    public OrderStatus Status { get; set; }
    public int UserId { get; set; }
    public int OrderAddressId {get; set; }

    public OrderAddress Address { get; set; } = default!;
    public User User { get; set; } = default!;
    public List<OrderItem> OrderItems { get; set; } = default!;
}