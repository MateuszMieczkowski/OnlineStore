using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Products.Repository;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<TaxRate>> GetTaxRatesAsync(CancellationToken cancellationToken = default);
}

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(OnlineStoreDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<TaxRate>> GetTaxRatesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TaxRate>()
            .ToListAsync(cancellationToken);
    }
}