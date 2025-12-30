using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.DeleteProduct;

public class HardDeleteProductsCommandHandler : ICommandHandler<HardDeleteProducts>
{
    private readonly OnlineStoreDbContext _dbContext;

    public HardDeleteProductsCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(HardDeleteProducts request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => request.Ids.Contains(x.Id) || request.DeleteAll)
            .ExecuteDeleteAsync(cancellationToken);
        
        await _dbContext.ProductFiles
            .Where(x => request.Ids.Contains(x.ProductId) || request.DeleteAll)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
