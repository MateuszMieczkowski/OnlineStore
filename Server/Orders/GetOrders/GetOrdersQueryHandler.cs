﻿using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Services;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Orders.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<Shared.Orders.GetOrders, PagedResult<OrderListItemDto>>
{
	private readonly OnlineStoreDbContext _dbContext;
	private readonly ILoggedUserService _loggedUserService;
	private readonly IResultPaginator _resultPaginator;

	public GetOrdersQueryHandler(
		OnlineStoreDbContext dbContext,
		ILoggedUserService loggedUserService,
		IResultPaginator resultPaginator)
	{
		_dbContext = dbContext;
		_loggedUserService = loggedUserService;
		_resultPaginator = resultPaginator;
	}

	public async Task<PagedResult<OrderListItemDto>> Handle(Shared.Orders.GetOrders query, CancellationToken cancellationToken)
	{
		var clientId = _loggedUserService.GetUserId();
		var orderStatuses = query.OrderStatuses?
			.Split(';')
			.Select(x => Enum.Parse<OrderStatus>(x));
		orderStatuses ??= Enum.GetValues<OrderStatus>();

		var dbQuery = _dbContext.Orders
			.AsNoTracking()
			.Where(x => x.ClientId == clientId
				&& orderStatuses.Contains(x.Status));

		var result = await _resultPaginator.GetPagedResult(dbQuery, query, x => x.ToListItemDto(), cancellationToken);
		return result;
	}
}
