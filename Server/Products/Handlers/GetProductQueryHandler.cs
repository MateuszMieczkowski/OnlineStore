using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Services;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Exceptions;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Handlers;

public class GetProductQueryHandler : IQueryHandler<GetProduct, ProductDto>
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILoggedUserService _loggedUserService;

    public GetProductQueryHandler(OnlineStoreDbContext dbContext, ILoggedUserService loggedUserService)
    {
        _dbContext = dbContext;
        _loggedUserService = loggedUserService;
    }

    public async Task<ProductDto> Handle(GetProduct query, CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Products
            .Where(x => x.Id == query.Id);

        if (_loggedUserService.GetUserRole() != UserRoles.Admin)
        {
           dbQuery = dbQuery.Where(x => !x.IsDeleted && !x.IsHidden);
        }
        
        var result = await dbQuery
            .Select(x => new ProductDto(
                x.Id,
                x.Name,
                x.ReferenceNumber,
                x.ShortDescription,
                x.Description ?? "",
                x.ThumbnailBlobUri,
                x.Quantity,
                x.PriceNet,
                x.PriceGross,
                x.IsHidden,
                x.IsDeleted,
                new TaxRateDto(x.TaxRate.Id, x.TaxRate.Amount, x.TaxRate.Description),
                x.ProductFiles.Select(y =>
                    new ProductFileDto(y.Id, y.FileName, y.BlobUri, y.Description, (ProductFileTypeDto)y.FileType))))
            .FirstOrDefaultAsync(cancellationToken) ??
                throw new NotFoundException($"Product with id {query.Id} not found");

        return result;
    }
}