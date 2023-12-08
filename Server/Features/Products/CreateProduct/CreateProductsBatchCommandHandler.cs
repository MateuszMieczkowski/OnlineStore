using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Features.Products.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.CreateProduct;

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
            var newProduct = await CreateProductAsync(productDto, taxRates, cancellationToken);
            _dbContext.Add(newProduct);
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
            throw new DuplicateException("One or more reference numbers already exists.");
        }
    }

    private async Task<Product> CreateProductAsync(CreateProductDto productDto, IEnumerable<TaxRate> taxRates, CancellationToken cancellationToken = default)
    {
        var taxRate = taxRates.FirstOrDefault(x => x.Id == productDto.TaxRateId)
            ?? throw new NotFoundException($"Tax rate with id {productDto.TaxRateId} was not found.");

        var product = new Product
        {
            Name = productDto.Name,
            ReferenceNumber = productDto.ReferenceNumber,
            Quantity = productDto.Quantity,
            PriceGross = _taxService.ApplyTax(productDto.PriceNet, taxRate.Amount),
            PriceNet = productDto.PriceNet,
            TaxRate = taxRate,
            IsHidden = productDto.IsHidden,
            IsDeleted = false,
            ShortDescription = productDto.ShortDescription,
            Description = productDto.Description,
            ProductFiles = new List<ProductFile>()
        };
        var productFileTasks = productDto.ProductFiles
            .Select(createProductFile => UploadProductFile(product, createProductFile, cancellationToken))
            .ToList();

        var productFiles = await Task.WhenAll(productFileTasks);
        foreach (var productFile in productFiles)
        {
            product.ProductFiles.Add(productFile);
        }

        return product;
    }
    
    private async Task<ProductFile> UploadProductFile(Product product, CreateProductFile createProductFile, CancellationToken cancellationToken = default)
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
            await _blobStorage.UploadAsync(new BlobFileName(productFile.BlobId, productFile.FileName), createProductFile.FileBase64, cancellationToken);

        if (productFile.FileType == ProductFileType.Thumbnail)
        {
            product.ThumbnailBlobUri = productFile.BlobUri;
        }

        return productFile;
    }
    
}