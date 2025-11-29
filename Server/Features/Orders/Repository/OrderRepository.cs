using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Orders.Repository;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OnlineStoreDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Order?> GetByIdAsync(int id,
        bool includeOrderItems = false,
        int? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Order>().AsQueryable();

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

    public async Task<(User client, OrderAddress Address)> GetExtendedDataByIdAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Users.FirstAsync(x => x.Id == order.ClientId, cancellationToken: cancellationToken);
        var address = await GetOrderAddressAsync(order.OrderAddressId, cancellationToken);

        return (client, address);
    }

    public async Task<OrderAddress> GetOrderAddressAsync(int orderAddressId, CancellationToken cancellationToken = default)
        => await _dbContext.OrdersAddresses.FirstAsync(x => x.Id == orderAddressId, cancellationToken: cancellationToken);
}