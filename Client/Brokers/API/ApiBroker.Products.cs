using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;
using ProductDto = OnlineStore.Shared.Models.ProductDto;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ProductRelativeUrl = "api/products";

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        return await GetAsync<List<ProductDto>>(ProductRelativeUrl);
    }

    
    public async Task PostProductsAsync(CreateProductsBatch command)
    {
        await PostAsync($"{ProductRelativeUrl}/create-batch", command);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto)
    {
        return await PutAsync<UpdateProductDto, ProductDto>(ProductRelativeUrl + $"/{id}", dto);
    }

    public async Task<bool> RemoveProductAsync(int id)
    {
        return await DeleteAsync(ProductRelativeUrl + $"/{id}");
    }
}