using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record GetOrders(int PageNumber, int PageSize, string? OrderStatuses, int? ClientId) : IPagedQuery<OrderListItemDto>;