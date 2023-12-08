using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Products.HideProduct;

public class HideProductCommandHandler : ICommandHandler<Shared.Products.HideProduct>
{
    private readonly OnlineStoreDbContext _dbContext;

    public HideProductCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(Shared.Products.HideProduct request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsHidden, true),
                cancellationToken);
    }
}