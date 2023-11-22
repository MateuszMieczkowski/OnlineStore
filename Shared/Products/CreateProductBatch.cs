using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record CreateProductsBatch(IReadOnlyCollection<CreateProductDto> Products) : ICommand;

public record CreateProductDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    bool IsHidden,
    int TaxRateId,
    IReadOnlyCollection<CreateProductFile> ProductFiles);
    
public record CreateProductFile(string FileName, string FileBase64, ProductFileTypeDto ProductFileType, string? Description);