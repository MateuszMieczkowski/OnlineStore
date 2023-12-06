using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public record GetOrder(int Id) : IQuery<OrderDto>;

