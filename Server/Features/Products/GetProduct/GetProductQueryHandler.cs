using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.GetProduct;

public class GetProductQueryHandler : IQueryHandler<Shared.Products.GetProduct, ProductDto>
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILoggedUserService _loggedUserService;

    public GetProductQueryHandler(OnlineStoreDbContext dbContext, ILoggedUserService loggedUserService)
    {
        _dbContext = dbContext;
        _loggedUserService = loggedUserService;
    }

    public async Task<ProductDto> Handle(Shared.Products.GetProduct query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Products
            .Include(x => x.TaxRate)
            .Where(x => x.Id == query.Id);

        if (_loggedUserService.GetUserRole() != UserRoles.Admin)
        {
           dbQuery = dbQuery.Where(x => !x.IsDeleted && !x.IsHidden);
        }
        
        var product = await dbQuery.FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Nie znaleziono produktu o ID {query.Id}");

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
            ProductFiles: product.ProductFiles.Select(p => new ProductFileDto(p.Id, p.FileName, p.BlobUri, p.Description, (ProductFileTypeDto)p.FileType)));
    }
}