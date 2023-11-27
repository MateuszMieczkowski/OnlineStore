using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ProductRelativeUrl = "api/products";

    public async Task<PagedResult<ProductListItemDto>> GetProductsAsync(GetProductList query)
    {
        var queryParams = $"?pageNumber={query.PageNumber}&pageSize={query.PageSize}&hiddenOnly={query.HiddenOnly}&deletedOnly={query.DeletedOnly}";
        Console.WriteLine(queryParams);
        return await GetAsync<PagedResult<ProductListItemDto>>($"{ProductRelativeUrl}{queryParams}");
    }

    public async Task<ProductDto> GetProductByIdAsync(GetProduct query)
    {
        return await GetAsync<ProductDto>($"{ProductRelativeUrl}/{query.Id}");
    }

    public async Task PostProductsAsync(CreateProductsBatch command)
    {
        await PostAsync($"{ProductRelativeUrl}/create-batch", command);
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto dto)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}", dto);
    }


    public async Task HideProductAsync(int id)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}/hide");
    }
    
    public async Task RecoverProductAsync(int id)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}/recover");
    }
    
    public async Task RevealProductAsync(int id)
    {
        await PutAsync($"{ProductRelativeUrl}/{id}/reveal");
    }

    public async Task SoftDeleteProductAsync(int id)
    {
        await DeleteAsync($"{ProductRelativeUrl}/{id}/soft-delete");
    }
    
    public async Task HardDeleteProductAsync(int id)
    {
        await DeleteAsync($"{ProductRelativeUrl}/{id}/hard-delete");
    }

    public async Task<IReadOnlyCollection<TaxRateDto>> GetTaxRates()
    {
        return await GetAsync<IReadOnlyCollection<TaxRateDto>>($"{ProductRelativeUrl}/taxRates");
    }
}