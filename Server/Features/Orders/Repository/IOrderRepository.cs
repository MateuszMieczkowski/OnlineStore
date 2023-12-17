using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Orders.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdAsync(int id,
        bool includeUser = false,
        bool includeOrderItems = false,
        int? userId = null,
        CancellationToken cancellationToken = default);
}