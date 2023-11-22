using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Products.Handlers;

internal class GetTaxRatesQueryHandler : IQueryHandler<GetTaxRates, IEnumerable<TaxRateDto>>
{
    private readonly OnlineStoreDbContext _dbContext;

    public GetTaxRatesQueryHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TaxRateDto>> Handle(GetTaxRates query, CancellationToken cancellationToken)
    {
        return await _dbContext.TaxRates
            .Select(x => new TaxRateDto (x.Id, x.Amount, x.Description))
            .ToListAsync(cancellationToken);
    }
}