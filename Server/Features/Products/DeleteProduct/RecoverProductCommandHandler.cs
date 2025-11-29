using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.DeleteProduct;

public class RecoverProductCommandHandler(OnlineStoreDbContext dbContext) : ICommandHandler<RecoverProduct>
{
    public async Task Handle(RecoverProduct request, CancellationToken token)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: token);
        if (product is null)
        {
            return;
        }

        product.IsDeleted = false;
        product.IsHidden = false;

        dbContext.Update(product);
        await dbContext.SaveChangesAsync(token);
    }
}