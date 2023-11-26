using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Products.Mapping;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Handlers;

public class GetProductsListQueryHandler : IQueryHandler<GetProductList, PagedResult<ProductListItemDto>>
{
    private readonly IResultPaginator _resultPaginator;
    private readonly OnlineStoreDbContext _dbContext;

    public GetProductsListQueryHandler(IResultPaginator resultPaginator, OnlineStoreDbContext dbContext)
    {
        _resultPaginator = resultPaginator;
        _dbContext = dbContext;
    }

    public async Task<PagedResult<ProductListItemDto>> Handle(GetProductList query, CancellationToken cancellationToken)
    {
        var dbQueryBase = _dbContext.Products
            .AsNoTracking();

        if (query.DeletedOnly)
        {
            dbQueryBase = dbQueryBase.Where(x => x.IsDeleted);
        }
        
        if (query.HiddenOnly)
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