using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record GetProductList(
    int PageNumber = 1,
    int PageSize = 20,
    bool DeletedOnly = false,
    bool HiddenOnly = false,
    string? SearchPhrase = null,
    string? Name = null,
    string? ReferenceNumber = null,
    string? ShortDescription = null,
    bool FilterGrossPrice = true,
    decimal? PriceFrom = null,
    decimal? PriceTo = null) : IPagedQuery<ProductListItemDto>;


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