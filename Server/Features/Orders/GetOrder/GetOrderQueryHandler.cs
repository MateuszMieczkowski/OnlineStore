using OnlineStore.Server.Authentication;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Features.Orders.Repository;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Features.Orders.GetOrder;

public class GetOrderQueryHandler(ILoggedUserService loggedUserService, IOrderRepository orderRepository) : IQueryHandler<Shared.Orders.GetOrder, OrderDto>
{
	public async Task<OrderDto> Handle(Shared.Orders.GetOrder query, CancellationToken cancellationToken)
	{
		var isClient = loggedUserService.GetUserRole() == UserRoles.User;
		int? userId = null;
		if (isClient)
		{
			 userId = loggedUserService.GetUserId();
		}
		
		var order = await orderRepository.GetByIdAsync(query.Id,
			includeOrderItems: true,
            userId: userId,
			cancellationToken: cancellationToken)
				?? throw new NotFoundException($"Nie znaleziono zamówienia o ID {query.Id}");
		var orderAddress = await orderRepository.GetOrderAddressAsync(order.OrderAddressId, cancellationToken); 
		var addressDto = new OrderAddressDto(
			Id: order.OrderAddressId,
			Street: orderAddress.Street,
			StreetNumber: orderAddress.StreetNumber,
			City: orderAddress.City,
			State: orderAddress.State,
			PostalCode: orderAddress.PostalCode,
			Country: orderAddress.Country);

		var orderItems = order.OrderItems
			.Select(x => new OrderItemDto(
				x.Id,
				x.PriceNet,
				x.PriceGross,
				x.Quantity,
				x.Product.Id,
				x.Product.Name,
				x.Product.ThumbnailBlobUri))
			.ToList();

		return new OrderDto(order.Id,
			order.TotalNet,
			order.TotalGross,
			(OrderStatusDto)order.Status,
			order.ClientId,
			order.CreatedDate,
			order.ModifiedDate,
			addressDto,
			orderItems);
	}
}
