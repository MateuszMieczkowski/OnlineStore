using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.DeleteProduct;

public class RecoverProductCommandHandler : ICommandHandler<RecoverProduct>
{
    private readonly OnlineStoreDbContext _dbContext;

    public RecoverProductCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RecoverProduct request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsDeleted, false)
                    .SetProperty(x => x.IsHidden, false),
                cancellationToken: cancellationToken);
    }
}