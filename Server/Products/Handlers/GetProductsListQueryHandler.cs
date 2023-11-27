using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Accounts.Services;
using OnlineStore.Server.Authentication;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Products.Mapping;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Server.Products.Handlers;

public class GetProductsListQueryHandler : IQueryHandler<GetProductList, PagedResult<ProductListItemDto>>
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

    public async Task<PagedResult<ProductListItemDto>> Handle(GetProductList query, CancellationToken cancellationToken)
    {
        
        bool isAdmin = _loggedUserService.GetUserRole() == UserRoles.Admin;
        
        var dbQueryBase = _dbContext.Products
            .AsNoTracking();
        
        if (!isAdmin)
        {
            dbQueryBase = dbQueryBase.Where(x => !x.IsHidden && !x.IsDeleted);
        }
        
        if (query is { DeletedOnly: true, HiddenOnly: false } && isAdmin)
        {
            dbQueryBase = dbQueryBase.Where(x => x.IsDeleted);
        }
        
        if (query is { HiddenOnly: true, DeletedOnly: false } && isAdmin)
        {
            dbQueryBase = dbQueryBase.Where(x => x.IsHidden);
        }
        
        if (!string.IsNullOrWhiteSpace(query.SearchPhrase))
        {
            var searchPhrase = query.SearchPhrase.Trim();
            dbQueryBase = dbQueryBase.Where(x => x.Name.Contains(searchPhrase) 
                || x.ReferenceNumber.Contains(searchPhrase)
                || x.ShortDescription!.Contains(searchPhrase)
                || x.Description!.Contains(searchPhrase));
        }

        if (query.PriceGrossFrom.HasValue)
        {
            dbQueryBase = dbQueryBase.Where(x => x.PriceGross >= query.PriceGrossFrom);
        }

        if (query.PriceGrossTo.HasValue)
        {
            dbQueryBase = dbQueryBase.Where(x => x.PriceGross <= query.PriceGrossTo);
        }
        
        var result = await _resultPaginator.GetPagedResult(dbQueryBase, query, x => x.ToListItemDto(), cancellationToken);
        return result;
    }
}