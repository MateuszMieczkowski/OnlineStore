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

    public async Task<ProductDto> Handle(Shared.Products.GetProduct query, CancellationToken token)
    {
        var dbQuery = _dbContext.Products.AsQueryable();

        if (_loggedUserService.GetUserRole() != UserRoles.Admin)
        {
           dbQuery = dbQuery.Where(x => !x.IsDeleted && !x.IsHidden);
        }

        var x = await dbQuery.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken: token);
        if (x is null)
        {
            throw new NotFoundException($"Nie znaleziono produktu o ID {query.Id}");
        }

        // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
        var taxRate = await _dbContext.TaxRates.FirstOrDefaultAsync(tr => tr.Id == x.TaxRateId, cancellationToken: token);
        // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
        var productFiles = x.ProductFiles
            .Select(y => new ProductFileDto(y.Id, y.FileName, y.BlobUri, y.Description, (ProductFileTypeDto)y.FileType))
            .ToList();
        
        return new ProductDto(
            Id: x.Id,
            Name: x.Name,
            ReferenceNumber: x.ReferenceNumber,
            ShortDescription: x.ShortDescription,
            Description: x.Description ?? "",
            ThumbnailUri: x.ThumbnailBlobUri,
            Quantity: x.Quantity,
            PriceNet: x.PriceNet,
            PriceGross: x.PriceGross,
            IsHidden: x.IsHidden,
            IsDeleted: x.IsDeleted,
            TaxRate: taxRate != null ? new TaxRateDto(taxRate.Id, taxRate.Amount, taxRate.Description) : null!,
            ProductFiles: productFiles);
    }
}