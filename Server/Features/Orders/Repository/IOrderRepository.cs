using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Orders.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithIncludedUserAsync(int id, CancellationToken cancellationToken = default);
}