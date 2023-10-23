using OnlineStore.Client.Brokers.API;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<bool> CreateProducts(List<CreateProductDto> dtos);
    Task<ProductDto> Update(int id, UpdateProductDto dto);
    Task<bool> Remove(int id);
}

public class ProductService : IProductService
{
    private readonly IApiBroker _broker;

    public ProductService(IApiBroker broker)
    {
        _broker = broker;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _broker.GetProductsAsync();
    }

    public async Task<bool> CreateProducts(List<CreateProductDto> dtos)
    {
        return await _broker.PostProductsAsync(dtos);
    }

    public async Task<ProductDto> Update(int id, UpdateProductDto dto)
    {
        return await _broker.UpdateProductAsync(id, dto);
    }

    public async Task<bool> Remove(int id)
    {
        return await _broker.RemoveProductAsync(id);
    }
}