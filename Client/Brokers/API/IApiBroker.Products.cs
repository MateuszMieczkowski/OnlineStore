using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<PagedResult<ProductListItemDto>> GetProductsAsync(GetProductList query);
    
    Task<ProductDto> GetProductByIdAsync(GetProduct query);

    Task PostProductsAsync(CreateProductsBatch command);
    Task UpdateProductAsync(int id, UpdateProductDto dto);
    Task HideProductAsync(int id);
    Task RevealProductAsync(int id);
    Task SoftDeleteProductAsync(int id);
    Task RecoverProductAsync(int id);
    Task HardDeleteProductAsync(int id);
    Task EmptyProductsBinAsync(IEnumerable<int> ids);

    Task<IReadOnlyCollection<TaxRateDto>> GetTaxRates();
}