using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record CreateOrder(int OrderAddressId, IReadOnlyCollection<CreateOrder.CreateOrderItem> Items) : ICreateCommand
{
    public record CreateOrderItem(int ProductId, int Count);

    public int CreatedId { get; set; }
};

