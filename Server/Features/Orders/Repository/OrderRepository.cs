using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Orders.Repository;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OnlineStoreDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Order?> GetByIdWithIncludedUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Order>()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}