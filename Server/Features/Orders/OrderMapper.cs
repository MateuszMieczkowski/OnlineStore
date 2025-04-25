using OnlineStore.Server.Entities;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Features.Orders;

public static class OrderMapper
{
	public static OrderListItemDto ToListItemDto(this Order order)
	{
		return new OrderListItemDto(
			Id: order.Id,
			TotalNet: order.TotalNet,
			TotalGross: order.TotalGross,
			ClientId: order.ClientId,
			ClientEmail: order.Client.Email,
			CreatedDate: order.CreatedDate)
		{
			ModifiedDate = order.ModifiedDate,
			Status = (OrderStatusDto)order.Status
		};
	}
}
