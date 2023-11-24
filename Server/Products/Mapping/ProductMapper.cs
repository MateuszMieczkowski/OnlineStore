using OnlineStore.Server.Entities;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Mapping;

public static class ProductMapper
{
    public static ProductListItemDto ToListItemDto(this Product product)
    {
        return new ProductListItemDto(
            Id: product.Id,
            Name: product.Name,
            ReferenceNumber: product.ReferenceNumber,
            ShortDescription: product.ShortDescription,
            Quantity: product.Quantity,
            PriceNet: product.PriceNet,
            PriceGross: product.PriceGross,
            ThumbnailUri: product.ThumbnailBlobUri);
    }
    
}