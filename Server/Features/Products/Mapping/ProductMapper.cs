using OnlineStore.Server.Entities;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.Mapping;

public static class ProductMapper
{
    public static ProductListItemDto ToListItemDto(this Product product)
    {
        return new ProductListItemDto(
            id: product.Id,
            name: product.Name,
            referenceNumber: product.ReferenceNumber,
            shortDescription: product.ShortDescription,
            quantity: product.Quantity,
            priceNet: product.PriceNet,
            priceGross: product.PriceGross,
            thumbnailUri: product.ThumbnailBlobUri)
        {
            Status = GetStatusDto(product)
        };
    }

    private static ProductStatusDto GetStatusDto(Product product)
    {
        if (product.IsDeleted)
        {
            return ProductStatusDto.Deleted;
        }

        if (product.IsHidden)
        {
            return ProductStatusDto.Hidden;
        }

        return ProductStatusDto.Active;
    }
}