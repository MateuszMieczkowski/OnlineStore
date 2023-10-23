using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<List<ProductDto>> GetProductsAsync();
    Task<bool> PostProductsAsync(List<CreateProductDto> dtos);
    Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto);
    Task<bool> RemoveProductAsync(int id);
}