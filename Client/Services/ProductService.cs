using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Products.Models;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;
using ProductDto = OnlineStore.Shared.Models.ProductDto;

namespace OnlineStore.Client.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task CreateProducts(ICollection<CreateProductModel> products);
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

    public async Task CreateProducts(ICollection<CreateProductModel> products)
    {
        var commandProducts = products.Select(
                x =>
                {
                    var files = x.ProductFiles
                        .Select(y => new CreateProductFile(y.FileName, y.FileBase64 ?? "", y.ProductFileType, y.Description))
                        .ToList();
                    return new ProductContracts(
                        x.Name,
                        x.ReferenceNumber,
                        x.ShortDescription,
                        x.Description,
                        x.Quantity,
                        x.PriceNet,
                        x.IsHidden,
                        (int)x.TaxRate,
                        files);
                })
            .ToList();

        var command = new CreateProductsBatch(commandProducts);
        
        await _broker.PostProductsAsync(command);
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