using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Orders.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdAsync(int id,
        bool includeOrderItems = false,
        int? userId = null,
        CancellationToken cancellationToken = default);

    Task<(User client, OrderAddress Address)> GetExtendedDataByIdAsync(
        Order order,
        CancellationToken cancellationToken = default);

    Task<OrderAddress> GetOrderAddressAsync(int orderAddressId, CancellationToken cancellationToken = default);
}