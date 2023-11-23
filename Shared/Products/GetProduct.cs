using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record GetProduct(int Id, bool IncludeDeleted, bool IncludeHidden) : IQuery<ProductDto>;

public record ProductDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string? Description,
    string ThumbnailUri,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    bool IsHidden,
    bool IsDeleted,
    TaxRateDto TaxRate,
    IEnumerable<ProductFileDto> ProductFiles) : ICommand;

public record ProductFileDto(int Id, string FileName, string BlobUri, string? Description, ProductFileTypeDto FileType);