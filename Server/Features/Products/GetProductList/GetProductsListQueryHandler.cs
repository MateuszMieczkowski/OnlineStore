using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Server.Features.Products.Mapping;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.GetProductList;

public class GetProductsListQueryHandler : IQueryHandler<Shared.Products.GetProductList, PagedResult<ProductListItemDto>>
{
    private readonly IResultPaginator _resultPaginator;
    private readonly OnlineStoreDbContext _dbContext;
    private ILoggedUserService _loggedUserService;

    public GetProductsListQueryHandler(
        IResultPaginator resultPaginator,
        OnlineStoreDbContext dbContext,
        ILoggedUserService loggedUserService)
    {
        _resultPaginator = resultPaginator;
        _dbContext = dbContext;
        _loggedUserService = loggedUserService;
    }

    public async Task<PagedResult<ProductListItemDto>> Handle(Shared.Products.GetProductList query, CancellationToken cancellationToken)
    {
        
        var isAdmin = _loggedUserService.GetUserRole() == UserRoles.Admin;
        
        var dbQueryBase = _dbContext.Products
            .AsNoTracking();
        
        if (query is { DeletedOnly: true, HiddenOnly: false } && isAdmin)
        {
            dbQueryBase = dbQueryBase.Where(x => x.IsDeleted);
        }
        
        if (query is { HiddenOnly: true, DeletedOnly: false } && isAdmin)
        {
            dbQueryBase = dbQueryBase.Where(x => x.IsHidden);
        }

        if (query is { HiddenOnly: false, DeletedOnly: false } || !isAdmin)
        {
            dbQueryBase = dbQueryBase.Where(x => !x.IsHidden && !x.IsDeleted);
        }
        
        if (!string.IsNullOrWhiteSpace(query.SearchPhrase))
        {
            var searchPhrase = query.SearchPhrase.Trim();
            dbQueryBase = dbQueryBase.Where(x => x.Name.Contains(searchPhrase) 
                || x.ReferenceNumber.Contains(searchPhrase)
                || x.ShortDescription!.Contains(searchPhrase)
                || x.Description!.Contains(searchPhrase));
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            dbQueryBase = dbQueryBase.Where(x => x.Name.Contains(query.Name));
        }
        
        if (!string.IsNullOrWhiteSpace(query.ReferenceNumber))
        {
            dbQueryBase = dbQueryBase.Where(x => x.ReferenceNumber.Contains(query.ReferenceNumber));
        }
        
        if (!string.IsNullOrWhiteSpace(query.ShortDescription))
        {
            dbQueryBase = dbQueryBase.Where(x => x.ShortDescription!.Contains(query.ShortDescription));
        }
        
        if (query.PriceFrom.HasValue)
        {
            dbQueryBase = query.FilterGrossPrice ? dbQueryBase.Where(x => x.PriceGross >= query.PriceFrom) : dbQueryBase.Where(x => x.PriceNet >= query.PriceFrom);
        }

        if (query.PriceTo.HasValue)
        {
            dbQueryBase = query.FilterGrossPrice ? dbQueryBase.Where(x => x.PriceGross <= query.PriceTo) : dbQueryBase.Where(x => x.PriceNet <= query.PriceTo);
        }
        
        var result = await _resultPaginator.GetPagedResult(dbQueryBase, query, x => x.ToListItemDto(), cancellationToken);
        return result;
    }
}