using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Features.Orders.Repository;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Features.Orders.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<Shared.Orders.GetOrder, OrderDto>
{
	private readonly ILoggedUserService _loggedUserService;
	private readonly IOrderRepository _orderRepository;

	public GetOrderQueryHandler(ILoggedUserService loggedUserService, IOrderRepository orderRepository)
	{
		_loggedUserService = loggedUserService;
		_orderRepository = orderRepository;
	}

	public async Task<OrderDto> Handle(Shared.Orders.GetOrder query, CancellationToken cancellationToken)
	{
		var isClient = _loggedUserService.GetUserRole() == UserRoles.User;
		int? userId = null;
		if (isClient)
		{
			 userId = _loggedUserService.GetUserId();
		}
		
		var order = await _orderRepository.GetByIdAsync(query.Id,
			includeUser: true,
			includeOrderItems: true,
            userId: userId,
			cancellationToken: cancellationToken)
				?? throw new NotFoundException($"Nie znaleziono zamówienia o ID {query.Id}");
		

		var addressDto = new OrderAddressDto(
			Id: order.OrderAddressId,
			Street: order.Address.Street,
			StreetNumber: order.Address.StreetNumber,
			City: order.Address.City,
			State: order.Address.State,
			PostalCode: order.Address.PostalCode,
			Country: order.Address.Country);

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
