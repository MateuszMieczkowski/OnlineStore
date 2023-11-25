using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<PagedResult<ProductListItemDto>> GetProductsAsync(GetProductList query);
    
    Task<ProductDto> GetProductByIdAsync(GetProduct query);

    Task PostProductsAsync(CreateProductsBatch command);
    Task UpdateProductAsync(int id, UpdateProductDto dto);
    Task<bool> RemoveProductAsync(int id);
}