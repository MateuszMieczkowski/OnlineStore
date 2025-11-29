using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.HideProduct;

public class RevealProductCommandHandler(OnlineStoreDbContext dbContext) : ICommandHandler<RevealProduct>
{
    public async Task Handle(RevealProduct request, CancellationToken token)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: token);
        if (product is null)
        {
            return;
        }

        product.IsHidden = false;

        dbContext.Update(product);
        await dbContext.SaveChangesAsync(token);
    }
}