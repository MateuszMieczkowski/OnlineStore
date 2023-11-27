using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Products.Models;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Services;

public interface IProductService
{
    Task<PagedResult<ProductListItemDto>> GetProductList(int pageNumber = 1,
        int pageSize = 50,
        bool deletedOnly = false,
        bool hiddenOnly = false,
        string? searchPhrase = null,
        decimal? priceGrossFrom = null,
        decimal? priceGrossTo = null);

    Task<ProductDto> GetProductById(int id);

    Task CreateProducts(ICollection<CreateProductModel> products);
    Task<ProductDtoOld> Update(int id, UpdateProductDto dto);
    Task<bool> Remove(int id);
}

public class ProductService : IProductService
{
    private readonly IApiBroker _broker;

    public ProductService(IApiBroker broker)
    {
        _broker = broker;
    }

    public async Task<PagedResult<ProductListItemDto>> GetProductList(
        int pageNumber = 1,
        int pageSize = 50,
        bool deletedOnly = false,
        bool hiddenOnly = false,
        string? searchPhrase = null,
        decimal? priceGrossFrom = null,
        decimal? priceGrossTo = null)
    {
        var query = new GetProductList(
            pageNumber,
            pageSize,
            deletedOnly,
            hiddenOnly,
            searchPhrase,
            priceGrossFrom,
            priceGrossTo);
        return await _broker.GetProductsAsync(query);
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var query = new GetProduct(Id: id, IncludeDeleted: true, IncludeHidden: true);
        return await _broker.GetProductByIdAsync(query);
    }

    public async Task CreateProducts(ICollection<CreateProductModel> products)
    {
        var commandProducts = products.Select(
                x =>
                {
                    var files = x.ProductFiles
                        .Select(y =>
                            new CreateProductFile(y.FileName, y.FileBase64 ?? "", y.ProductFileType, y.Description))
                        .ToList();
                    return new CreateProductDto(
                        Name: x.Name,
                        ReferenceNumber: x.ReferenceNumber,
                        ShortDescription: x.ShortDescription,
                        Description: x.Description,
                        Quantity: x.Quantity,
                        PriceNet: x.PriceNet,
                        IsHidden: x.IsHidden,
                        TaxRateId: (int)x.TaxRate,
                        ProductFiles: files);
                })
            .ToList();

        var command = new CreateProductsBatch(commandProducts);

        await _broker.PostProductsAsync(command);
    }

    public async Task<ProductDtoOld> Update(int id, UpdateProductDto dto)
    {
        return await _broker.UpdateProductAsync(id, dto);
    }

    public async Task<bool> Remove(int id)
    {
        return await _broker.RemoveProductAsync(id);
    }
}