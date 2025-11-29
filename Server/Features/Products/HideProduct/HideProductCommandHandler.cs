using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.Products.HideProduct;

public class HideProductCommandHandler(OnlineStoreDbContext dbContext) : ICommandHandler<Shared.Products.HideProduct>
{
    public async Task Handle(Shared.Products.HideProduct request, CancellationToken token)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: token);
        if (product is null)
        {
            return;
        }

        product.IsHidden = true;

        dbContext.Update(product);
        await dbContext.SaveChangesAsync(token);
    }
}