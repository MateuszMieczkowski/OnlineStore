using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record UpdateProduct(
    int Id,
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    bool IsHidden,
    bool IsDeleted,
    int TaxRateId,
    IReadOnlyCollection<UpdateProductFileDto> ProductFiles) : ICommand;

public record UpdateProductDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    decimal PriceGross,
    bool IsHidden,
    bool IsDeleted,
    int TaxRateId,
    IReadOnlyCollection<UpdateProductFileDto> ProductFiles)
{
    public UpdateProduct ToCommand(int productId) =>
        new(productId,
            Name,
            ReferenceNumber,
            ShortDescription,
            Description,
            Quantity,
            PriceNet,
            PriceGross,
            IsHidden,
            IsDeleted,
            TaxRateId,
            ProductFiles);
};

public record UpdateProductFileDto(
    int? Id,
    string FileName,
    string FileBase64,
    ProductFileTypeDto ProductFileType,
    string? Description);

    
    