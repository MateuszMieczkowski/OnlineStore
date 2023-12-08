using OnlineStore.Server.Entities;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.Mapping;

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
            ThumbnailUri: product.ThumbnailBlobUri)
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