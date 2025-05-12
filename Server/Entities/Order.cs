using OnlineStore.Server.Entities.Abstractions;
using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class Order :
    ITimeModified,
    ITimeCreated
{
    public int Id { get; set; }
    public decimal TotalNet { get; set; }
    public decimal TotalGross { get; set; }
    public OrderStatus Status { get; set; }
    public int ClientId { get; set; }
    public int OrderAddressId { get; set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    // public User User { get; set; } = default!;
    public OrderAddress Address { get; set; } = default!;
    public Client Client { get; set; } = default!;
    public List<OrderItem> OrderItems { get; set; } = default!;
}