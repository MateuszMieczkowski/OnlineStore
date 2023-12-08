using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Features.Orders.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<Shared.Orders.GetOrder, OrderDto>
{
	private readonly ILoggedUserService _loggedUserService;
	private readonly OnlineStoreDbContext _dbContext;

	public GetOrderQueryHandler(ILoggedUserService loggedUserService, OnlineStoreDbContext dbContext)
	{
		_loggedUserService = loggedUserService;
		_dbContext = dbContext;
	}

	public async Task<OrderDto> Handle(Shared.Orders.GetOrder query, CancellationToken cancellationToken)
	{
		var dbQueryBase = _dbContext.Orders
			.AsNoTracking()
			.Include(x => x.OrderItems)
			.Include(x => x.Address)
			.Where(x => x.Id == query.Id);

		var isClient = _loggedUserService.GetUserRole() == UserRoles.Admin;
		if (isClient)
		{
			var clientId = _loggedUserService.GetUserId();
			dbQueryBase = dbQueryBase.Where(x => x.ClientId == clientId);
		}

		var order = await dbQueryBase.FirstOrDefaultAsync(cancellationToken)
			?? throw new NotFoundException($"Order with id {query.Id} not found");

		var addressDto = new OrderAddressDto(
			order.OrderAddressId,
			order.Address.StreetNumber,
			order.Address.City,
			order.Address.State,
			order.Address.PostalCode,
			order.Address.Country);

		var orderItems = order.OrderItems
			.Select(x => new OrderItemDto(
				x.Id,
				x.PriceNet,
				x.PriceGross,
				x.Quantity,
				x.Product.Id))
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
