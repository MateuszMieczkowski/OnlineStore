using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Handlers;

public class HideProductCommandHandler : ICommandHandler<HideProduct>
{
    private readonly OnlineStoreDbContext _dbContext;

    public HideProductCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(HideProduct request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsHidden, true),
                cancellationToken);
    }
}