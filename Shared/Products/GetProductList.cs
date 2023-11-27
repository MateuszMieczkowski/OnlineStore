using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record GetProductList(
    int PageNumber,
    int PageSize,
    bool DeletedOnly = false,
    bool HiddenOnly = false,
    string? SearchPhrase = null,
    decimal? PriceGrossFrom = null,
    decimal? PriceGrossTo = null) : IPagedQuery<ProductListItemDto>;


public record ProductListItemDto(
    int Id,
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    string ThumbnailUri)
{
    public ProductStatusDto Status { get; set; }
};