using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Products.Models;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Services;

public interface IProductService
{
    Task<PagedResult<ProductListItemDto>> GetProductList(int pageNumber, int pageSize, ProductFilterModel filter);

    Task<ProductDto> GetProductById(int id);

    Task CreateProducts(ICollection<CreateProductModel> products);
    Task Update(int id, UpdateProductModel model);
    Task SoftDelete(int id);
    Task Hide(int id);
    Task Reveal(int id);
    Task Recover(int id);
    Task HardDelete(int id);
    Task EmptyBin(IEnumerable<int> ids);

    Task<IReadOnlyCollection<TaxRateDto>> GetTaxRates();
}

public class ProductService : IProductService
{
    private readonly IApiBroker _broker;

    public ProductService(IApiBroker broker)
    {
        _broker = broker;
    }

    public async Task<PagedResult<ProductListItemDto>> GetProductList(
        int pageNumber,
        int pageSize,
        ProductFilterModel filter)
    {
        var query = new GetProductList(
            PageNumber: pageNumber,
            PageSize: pageSize,
            DeletedOnly: filter.DeletedOnly,
            HiddenOnly: filter.HiddenOnly,
            SearchPhrase: filter.SearchPhrase,
            Name: filter.Name,
            ReferenceNumber: filter.ReferenceNumber,
            ShortDescription: filter.ShortDescription,
            FilterGrossPrice: filter.FilterGrossPrice,
            PriceFrom: filter.MinPrice,
            PriceTo: filter.MaxPrice);
        
        return await _broker.GetProductsAsync(query);
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var query = new GetProduct(Id: id);
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
                        TaxRateId: x.TaxRate!.TaxRateId,
                        ProductFiles: files);
                })
            .ToList();

        var command = new CreateProductsBatch(commandProducts);

        await _broker.PostProductsAsync(command);
    }

    public async Task Update(int id, UpdateProductModel model)
    {
        var dtoFiles = model.ProductFiles.Select(x => new UpdateProductFileDto(x.Id, x.FileName, x.FileBase64, x.ProductFileType, x.Description)).ToList();
        var dto = new UpdateProductDto(
            Name: model.Name,
            ReferenceNumber: model.ReferenceNumber,
            ShortDescription: model.ShortDescription,
            Description: model.Description,
            Quantity: model.Quantity,
            PriceNet: model.PriceNet,
            IsHidden: model.IsHidden,
            IsDeleted: false,
            TaxRateId: model.TaxRate!.TaxRateId,
            ProductFiles: dtoFiles);
        
        await _broker.UpdateProductAsync(id, dto);
    }

    public async Task SoftDelete(int id)
        => await _broker.SoftDeleteProductAsync(id);

    public async Task Hide(int id)
        => await _broker.HideProductAsync(id);

    public async Task Reveal(int id)
        => await _broker.RevealProductAsync(id);

    public async Task Recover(int id)
        => await _broker.RecoverProductAsync(id);

    public async Task HardDelete(int id)
        => await _broker.HardDeleteProductAsync(id);

    public async Task EmptyBin(IEnumerable<int> ids) => await _broker.EmptyProductsBinAsync(ids);

    public async Task<IReadOnlyCollection<TaxRateDto>> GetTaxRates()
        => await _broker.GetTaxRates();
}