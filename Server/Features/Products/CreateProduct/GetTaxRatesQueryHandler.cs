using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Features.Products.Repository;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.CreateProduct;

internal class GetTaxRatesQueryHandler : IQueryHandler<GetTaxRates, IEnumerable<TaxRateDto>>
{
    private readonly IProductRepository _productRepository;
    public GetTaxRatesQueryHandler( IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<TaxRateDto>> Handle(GetTaxRates query, CancellationToken cancellationToken)
    {
        var taxRates = await _productRepository.GetTaxRatesAsync(cancellationToken: cancellationToken);

        return taxRates.Select(x => new TaxRateDto(x.Id, x.Amount, x.Description));
    }
}