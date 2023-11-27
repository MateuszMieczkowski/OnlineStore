using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Handlers;

public class SoftDeleteProductCommandHandler : ICommandHandler<SoftDeleteProduct>
{
    private readonly OnlineStoreDbContext _dbContext;

    public SoftDeleteProductCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SoftDeleteProduct request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsDeleted, true)
                    .SetProperty(x => x.IsHidden, false),
                cancellationToken: cancellationToken);
    }
}