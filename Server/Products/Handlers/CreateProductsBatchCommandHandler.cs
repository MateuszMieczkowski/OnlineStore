using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Products.Services;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Handlers;

public class CreateProductsBatchCommandHandler : ICommandHandler<CreateProductsBatch>
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ITaxService _taxService;
    private readonly IBlobStorage _blobStorage;

    public CreateProductsBatchCommandHandler(
        OnlineStoreDbContext dbContext,
        ITaxService taxService,
        IBlobStorage blobStorage)
    {
        _dbContext = dbContext;
        _taxService = taxService;
        _blobStorage = blobStorage;
    }

    public async Task Handle(CreateProductsBatch command, CancellationToken cancellationToken)
    {
        var taxRates = await _dbContext.TaxRates
            .ToListAsync(cancellationToken);
        
        await ValidateReferenceNumbers(command, cancellationToken);

        foreach (var productDto in command.Products)
        {
            await CreateProductAsync(productDto, taxRates);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateReferenceNumbers(CreateProductsBatch command, CancellationToken cancellationToken)
    {
        var newReferenceNumbers = command.Products
            .Select(x => x.ReferenceNumber);
        
        var anyReferenceNumberExists = await _dbContext.Products
            .AnyAsync(x => newReferenceNumbers.Contains(x.ReferenceNumber), cancellationToken);
        
        if (anyReferenceNumberExists)
        {
            throw new BadRequestException("One or more reference numbers already exists.");
        }
    }

    private async Task CreateProductAsync(CreateProductDto productDto, IEnumerable<TaxRate> taxRates)
    {
        var taxRate = taxRates.FirstOrDefault(x => x.Id == productDto.TaxRateId)
            ?? throw new NotFoundException($"Tax rate with id {productDto.TaxRateId} was not found.");

        var product = new Product
        {
            Name = productDto.Name,
            ReferenceNumber = productDto.ReferenceNumber,
            Quantity = productDto.Quantity,
            PriceGross = productDto.PriceGross,
            PriceNet = _taxService.RemoveTax(productDto.PriceGross,
                taxRate.Amount),
            TaxRate = taxRate,
            IsHidden = productDto.IsHidden,
            IsDeleted = false,
            ShortDescription = productDto.ShortDescription,
            Description = productDto.Description,
        };

        foreach (var createProductFile in productDto.ProductFiles)
        {
            await UploadProductFile(product, createProductFile);
        }
    }
    
    private async Task UploadProductFile(Product product, CreateProductFile createProductFile)
    {
        var productFile = new ProductFile
        {
            BlobId = Guid.NewGuid(),
            FileName = createProductFile.FileName,
            Description = createProductFile.Description,
            Product = product,
            FileType = (ProductFileType)createProductFile.ProductFileType,
        };

        productFile.BlobUri =
            await _blobStorage.UploadAsync(productFile.BlobId, productFile.FileName, createProductFile.FileBase64);

        if (productFile.FileType == ProductFileType.Thumbnail)
        {
            product.ThumbnailBlobUri = productFile.BlobUri;
        }

        await _dbContext.AddAsync(productFile);
    }
    
}