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

    public GetProductsListQueryHandler(IResultPaginator resultPaginator, OnlineStoreDbContext dbContext)
    {
        _resultPaginator = resultPaginator;
        _dbContext = dbContext;
    }

    public async Task<PagedResult<ProductListItemDto>> Handle(GetProductList query, CancellationToken cancellationToken)
    {
        var dbQueryBase = _dbContext.Products
            .Where(x => (query.IncludeDeleted || !x.IsDeleted) 
                && (query.IncludeHidden || !x.IsHidden));
        
        var result = await _resultPaginator.GetPagedResult(dbQueryBase, query, x => x.ToListItemDto(), cancellationToken);
        return result;
    }
}