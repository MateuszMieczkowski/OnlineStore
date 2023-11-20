using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;


// GET: api/products (filtry się dorobi później)
public record GetProductList(int PageNumber, int PageSize) : IPagedQuery<ProductListItemDto>;

public record ProductListItemDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    string ThumbnailPath);


// GET: api/products/{id}
public record GetProduct(int Id) : IQuery<ProductDto>;

public record ProductDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    bool IsHidden,
    TaxRateDto TaxRate,
    IReadOnlyCollection<ProductFileDto> ProductFiles) : ICommand;

public record TaxRateDto(int TaxRateId, int Amount, string Description);

public record ProductFileDto(int Id, string FileName, string BlobUri, string? Description);

// POST: api/products/create-batch
public record CreateProductsBatch(IReadOnlyCollection<ProductContracts> Products);

public record ProductContracts(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    bool IsHidden,
    int TaxRateId,
    IReadOnlyCollection<CreateProductFile> ProductFiles) : ICommand;

public record CreateProductFile(string FileName, string FileBase64, ProductFileTypeDto ProductFileType, string? Description);

public enum ProductFileTypeDto
{
    Thumbnail = 1,
    Image = 2,
    Other = 3,
}

// PUT: api/products/{id}/update
public record UpdateProduct(
    int Id,
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    bool IsHidden,
    int TaxRateId,
    IReadOnlyCollection<CreateProductFile> ProductFiles) : ICommand;
    
// PUT: api/products/{id}/recover (after soft-delete)
public record RecoverProduct(int Id) : ICommand;

// DELETE: api/products/soft-delete/{id}
public record SoftDeleteProduct(int Id) : ICommand;

// DELETE: api/products/hard-delete/{id}
public record HardDeleteProduct(int Id) : ICommand;