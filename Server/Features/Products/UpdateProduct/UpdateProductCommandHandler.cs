using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Features.Products.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.UpdateProduct;

public class UpdateProductCommandHandler : ICommandHandler<Shared.Products.UpdateProduct>
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly IBlobStorage _blobStorage;
    private readonly ITaxService _taxService;

    public UpdateProductCommandHandler(OnlineStoreDbContext dbContext, IBlobStorage blobStorage, ITaxService taxService)
    {
        _dbContext = dbContext;
        _blobStorage = blobStorage;
        _taxService = taxService;
    }

    public async Task Handle(Shared.Products.UpdateProduct command, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Include(x => x.ProductFiles)
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken) ??
                throw new NotFoundException($"Product with id {command.Id} was not found.");
        
        await ValidateProductReferenceNumberAsync(command.Id, command.ReferenceNumber, cancellationToken);
        
        var taxRate = await _dbContext.TaxRates
            .FirstOrDefaultAsync(x => x.Id == command.TaxRateId, cancellationToken: cancellationToken) ??
                throw new NotFoundException($"Tax rate with id {command.TaxRateId} was not found.");

        product.Name = command.Name;
        product.ReferenceNumber = command.ReferenceNumber;
        product.ShortDescription = command.ShortDescription;
        product.Description = command.Description;
        product.Quantity = command.Quantity;
        product.IsHidden = command.IsHidden;
        product.IsDeleted = command.IsDeleted;
        product.PriceNet = command.PriceNet;
        product.PriceGross = _taxService.ApplyTax(product.PriceNet, taxRate.Amount);
        
        foreach (var productFile in product.ProductFiles)
        {
            await UpdateProductFiles(command, productFile, product, cancellationToken);
        }

        foreach (var newProductFile in command.ProductFiles.Where(x => x.Id is null))
        {
            await CreateProductFileAsync(product, newProductFile, cancellationToken);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateProductFileAsync(Product product, UpdateProductFileDto productFileDto, CancellationToken cancellationToken)
    {
        var newProductFile = new ProductFile
        {
            Product = product,
            FileName = productFileDto.FileName,
            Description = productFileDto.Description,
            FileType = (ProductFileType)productFileDto.ProductFileType,
            BlobId = Guid.NewGuid()
        };
        newProductFile.BlobUri = await _blobStorage.UploadAsync(new BlobFileName(newProductFile.BlobId, newProductFile.FileName),
            productFileDto.FileBase64, cancellationToken);
        
        if (newProductFile.FileType == ProductFileType.Thumbnail)
        {
            product.ThumbnailBlobUri = newProductFile.BlobUri;
        }
        
        product.ProductFiles.Add(newProductFile);
    }

    private async Task ValidateProductReferenceNumberAsync(int productId, string productReferenceNumber, CancellationToken cancellationToken = default)
    {
        var referenceNumberExists = await _dbContext.Products
            .Where(x => x.Id != productId
                && x.ReferenceNumber == productReferenceNumber)
            .AnyAsync(cancellationToken);
        
        if (referenceNumberExists)
        {
            throw new DuplicateException($"Product with reference number {productReferenceNumber} already exists.");
        }
    }

    private async Task UpdateProductFiles(Shared.Products.UpdateProduct request, ProductFile productFile,
        Product product, CancellationToken cancellationToken)
    {
        var updateProductFileDto = request.ProductFiles.FirstOrDefault(x => x.Id == productFile.Id);
        if (updateProductFileDto is null)
        {
            _dbContext.Remove(productFile);
            await _blobStorage.RemoveAsync(new BlobFileName(productFile.BlobId, productFile.FileName),
                cancellationToken);
            return;
        }

        productFile.FileName = updateProductFileDto.FileName;
        productFile.Description = updateProductFileDto.Description;
        
        if (updateProductFileDto.FileBase64 != null)
        {
            productFile.FileType = (ProductFileType)updateProductFileDto.ProductFileType;
            productFile.BlobUri = await _blobStorage.UploadAsync(new BlobFileName(productFile.BlobId, productFile.FileName),
            updateProductFileDto.FileBase64, cancellationToken);
        }
        
        if (productFile.FileType == ProductFileType.Thumbnail)
        {
            product.ThumbnailBlobUri = productFile.BlobUri;
        }
    }
}