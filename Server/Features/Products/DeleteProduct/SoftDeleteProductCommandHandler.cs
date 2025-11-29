using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.DeleteProduct;

public class SoftDeleteProductCommandHandler(OnlineStoreDbContext dbContext) : ICommandHandler<SoftDeleteProduct>
{
    public async Task Handle(SoftDeleteProduct request, CancellationToken token)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: token);
        if (product is null)
        {
            return;
        }

        product.IsHidden = false;
        product.IsDeleted = true;

        dbContext.Update(product);
        await dbContext.SaveChangesAsync(token);
    }
}