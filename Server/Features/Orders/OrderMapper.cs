using OnlineStore.Server.Entities;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Features.Orders;

public static class OrderMapper
{
	public static OrderListItemDto ToListItemDto(this Order order)
	{
		return new OrderListItemDto(
			id: order.Id,
			totalNet: order.TotalNet,
			totalGross: order.TotalGross,
			clientId: order.ClientId,
			clientEmail: order.Client.Email,
			createdDate: order.CreatedDate)
		{
			ModifiedDate = order.ModifiedDate,
			Status = (OrderStatusDto)order.Status
		};
	}
}
