using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Orders.Repository;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OnlineStoreDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Order?> GetByIdAsync(int id,
        bool includeUser = false,
        bool includeOrderItems = false,
        int? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Order>()
            .Include(x => x.Address)
            .AsQueryable();
        if (includeUser)
        {
            query = query.Include(x => x.Client);
        }

        if (includeOrderItems)
        {
            query = query.Include(x => x.OrderItems);
        }

        if (userId is not null)
        {
            query = query.Where(x => x.ClientId == userId);
        }
        
        return await query
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}