using OnlineStore.Client.Products.Models;
using OnlineStore.Client.Services;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.SoapServices;

public class ProductSoapService(ISoapClient soapClient) : IProductService
{
    private const string Endpoint = "/soap/product";
    private const string ServiceNamespace = "http://tempuri.org/IProductSoapService";

    public async Task<PagedResult<ProductListItemDto>> GetProductList(int pageNumber, int pageSize, ProductFilterModel filter)
    {
        var query = new GetProductList(
            pageNumber: pageNumber,
            pageSize: pageSize,
            deletedOnly: filter.DeletedOnly,
            hiddenOnly: filter.HiddenOnly,
            searchPhrase: filter.SearchPhrase,
            name: filter.Name,
            referenceNumber: filter.ReferenceNumber,
            shortDescription: filter.ShortDescription,
            filterGrossPrice: filter.FilterGrossPrice,
            priceFrom: filter.MinPrice,
            priceTo: filter.MaxPrice);

        var result = await soapClient.SendQuery<GetProductList, ProductListPagedResponseDto>(
            Endpoint,
            ServiceNamespace,
            nameof(GetProductList),
            query);

        return new PagedResult<ProductListItemDto>(
                result.Items,
                result.PageNumber,
                result.PageSize,
                result.TotalPages,
                result.TotalItemsCount);
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        return await soapClient.SendQuery<GetProduct, ProductDto>(
            Endpoint,
            ServiceNamespace,
            nameof(GetProduct),
            new GetProduct(id));
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
                        name: x.Name,
                        referenceNumber: x.ReferenceNumber,
                        shortDescription: x.ShortDescription,
                        description: x.Description,
                        quantity: x.Quantity,
                        priceNet: x.PriceNet,
                        isHidden: x.IsHidden,
                        taxRateId: x.TaxRate!.TaxRateId,
                        productFiles: files);
                })
            .ToList();

        var command = new CreateProductsBatch(commandProducts);

        await soapClient.SendCommand(Endpoint, ServiceNamespace, nameof(CreateProductsBatch), command);
    }

    public async Task Update(int id, UpdateProductModel model)
    {
        var dtoFiles = model.ProductFiles.Select(x => new UpdateProductFileDto(x.Id, x.FileName, x.FileBase64, x.ProductFileType, x.Description)).ToList();
        var command = new UpdateProduct(
            id: id,
            name: model.Name,
            referenceNumber: model.ReferenceNumber,
            shortDescription: model.ShortDescription,
            description: model.Description,
            quantity: model.Quantity,
            priceNet: model.PriceNet,
            isHidden: model.IsHidden,
            isDeleted: false,
            taxRateId: model.TaxRate!.TaxRateId,
            productFiles: dtoFiles);

        await soapClient.SendCommand(Endpoint, ServiceNamespace, nameof(UpdateProduct), command);
    }

    public Task SoftDelete(int id)
        => soapClient.SendCommand(
            Endpoint,
            ServiceNamespace,
            nameof(SoftDeleteProduct),
            new SoftDeleteProduct(id));

    public Task Hide(int id)
        =>  soapClient.SendCommand(
            Endpoint,
            ServiceNamespace,
            nameof(HideProduct),
            new HideProduct(id));

    public Task Reveal(int id)
        => soapClient.SendCommand(
            Endpoint,
            ServiceNamespace,
            nameof(RevealProduct),
            new RevealProduct(id));

    public Task Recover(int id)
        => soapClient.SendCommand(
            Endpoint,
            ServiceNamespace,
            nameof(RecoverProduct),
            new RecoverProduct(id));

    public Task HardDelete(int id)
        => soapClient.SendCommand(
            Endpoint,
            ServiceNamespace,
            nameof(HardDeleteProduct),
            new HardDeleteProduct(id));

    public Task EmptyBin(IEnumerable<int> ids)
        => Task.CompletedTask;

    public async Task<IReadOnlyCollection<TaxRateDto>> GetTaxRates()
    {
        var result = await soapClient.SendQuery<GetTaxRates, TaxRateListResponseDto>(
            Endpoint,
            ServiceNamespace,
            nameof(GetTaxRates),
            null);
        return result.TaxRates;
    }
}
