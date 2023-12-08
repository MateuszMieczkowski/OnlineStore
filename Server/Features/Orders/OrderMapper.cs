using OnlineStore.Server.Entities;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Features.Orders;

public static class OrderMapper
{
	public static OrderListItemDto ToListItemDto(this Order order)
	{
		return new OrderListItemDto(
			order.Id,
			order.TotalGross,
			order.TotalNet,
			(OrderStatusDto)order.Status,
			order.ClientId,
			order.CreatedDate,
			order.ModifiedDate);
	}
}
