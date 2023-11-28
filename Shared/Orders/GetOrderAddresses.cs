using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record GetOrderAddresses(int PageNumber, int PageSize) : IPagedQuery<OrderAddressDto>;