using OnlineStore.Server.Entities;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Mapping;

public static class ProductMapper
{
    public static ProductListItemDto ToListItemDto(this Product product)
    {
        return new ProductListItemDto(
            product.Id,
            product.Name,
            product.ReferenceNumber,
            product.ShortDescription,
            product.Quantity,
            product.PriceNet,
            product.PriceGross,
            product.ThumbnailBlobUri);
    }
    
}