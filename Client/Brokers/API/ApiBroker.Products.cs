using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ProductRelativeUrl = "api/products";

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        return await GetAsync<List<ProductDto>>(ProductRelativeUrl);
    }

    public async Task<bool> PostProductsAsync(List<CreateProductDto> dtos)
    {
        return await PostAsync(ProductRelativeUrl, dtos);
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