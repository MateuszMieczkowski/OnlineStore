using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record GetAllOrders(int PageNumber, int PageSize, int? UserId, string? OrderStatuses) : IPagedQuery<OrderListItemDto>;