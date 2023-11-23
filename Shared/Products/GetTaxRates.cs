using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Products;

public record GetTaxRates : IQuery<IEnumerable<TaxRateDto>>;

public record TaxRateDto(int TaxRateId, int Amount, string Description);
