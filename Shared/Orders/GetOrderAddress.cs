using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record GetOrderAddress : IQuery<OrderAddressDto?>;