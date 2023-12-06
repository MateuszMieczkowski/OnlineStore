using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record CreateOrder(int OrderAddressId, IReadOnlyCollection<CreateOrder.CreateOrderItem> Items) : ICommand
{
	public record CreateOrderItem(int ProductId, int Count);
};
