using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.DeleteProduct;

public class HardDeleteProductCommandHandler : ICommandHandler<HardDeleteProduct>
{
    private readonly OnlineStoreDbContext _dbContext;

    public HardDeleteProductCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(HardDeleteProduct request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.Id && x.IsDeleted)
            .ExecuteDeleteAsync(cancellationToken);
    }
}