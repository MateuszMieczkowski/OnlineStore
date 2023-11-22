using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;
using ProductDto = OnlineStore.Shared.Models.ProductDto;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<List<ProductDto>> GetProductsAsync();

    Task PostProductsAsync(CreateProductsBatch command);
    Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto);
    Task<bool> RemoveProductAsync(int id);
}