using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Server.ThumbnailServiceImplService;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.GetProduct;

public class GetProductQueryHandler : IQueryHandler<Shared.Products.GetProduct, ProductDto>
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILoggedUserService _loggedUserService;
    private readonly ThumbnailService _thumbnailService;

    public GetProductQueryHandler(OnlineStoreDbContext dbContext, ILoggedUserService loggedUserService, ThumbnailService thumbnailService)
    {
        _dbContext = dbContext;
        _loggedUserService = loggedUserService;
        _thumbnailService = thumbnailService;
    }

    public async Task<ProductDto> Handle(Shared.Products.GetProduct query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Products
            .Include(x => x.TaxRate)
            .Include(x => x.ProductFiles)
            .Where(x => x.Id == query.Id);

        if (_loggedUserService.GetUserRole() != UserRoles.Admin)
        {
           dbQuery = dbQuery.Where(x => !x.IsDeleted && !x.IsHidden);
        }

        var product = await dbQuery.FirstOrDefaultAsync(cancellationToken) ??
                throw new NotFoundException($"Nie znaleziono produktu o ID {query.Id}");

        var thumbnail = await GetThumbnail(product);

        return new ProductDto(
            Id: product.Id,
            Name: product.Name,
            ReferenceNumber: product.ReferenceNumber,
            ShortDescription: product.ShortDescription,
            Description: product.Description ?? "",
            ThumbnailUri: product.ThumbnailBlobUri,
            Quantity: product.Quantity,
            PriceNet: product.PriceNet,
            PriceGross: product.PriceGross,
            IsHidden: product.IsHidden,
            IsDeleted: product.IsDeleted,
            TaxRate: new TaxRateDto(product.TaxRate.Id, product.TaxRate.Amount, product.TaxRate.Description),
            ProductFiles: product.ProductFiles.Select(y =>
                new ProductFileDto(y.Id, y.FileName, y.BlobUri, y.Description, (ProductFileTypeDto)y.FileType)),
            Thumbnail: thumbnail);
    }

    private async Task<string?> GetThumbnail(Product product)
    {
        try
        {
            var productId = Path.GetFileNameWithoutExtension(product.ThumbnailBlobUri);
            var body = new downloadThumbnailRequestBody(productId);
            var request = new downloadThumbnailRequest(body);
            var thumbnailResponse = await _thumbnailService.downloadThumbnailAsync(request);
            var thumbnail = Convert.ToBase64String(thumbnailResponse.Body.@return);
            return thumbnail;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return null;
    }
}