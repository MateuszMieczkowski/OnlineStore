using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record GetProductList(int PageNumber, int PageSize, bool IncludeDeleted, bool IncludeHidden) : IPagedQuery<ProductListItemDto>;

public record ProductListItemDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    string ThumbnailUri);