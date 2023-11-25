using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ProductRelativeUrl = "api/products";

    public async Task<PagedResult<ProductListItemDto>> GetProductsAsync(GetProductList query)
    {
        var queryParams = $"?pageNumber={query.PageNumber}&pageSize={query.PageSize}";
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

    public async Task<bool> RemoveProductAsync(int id)
    {
        return await DeleteAsync(ProductRelativeUrl + $"/{id}");
    }
}