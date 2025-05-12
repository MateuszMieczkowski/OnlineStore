using MediatR;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;
using OnlineStore.Shared.SoapContracts;

namespace OnlineStore.Server.SoapServices;

public class ProductSoapService(IMediator mediator) : IProductSoapService
{
    public Task CreateProductsBatch(CreateProductsBatch command)
        => mediator.Send(command);

    public async Task<ProductListPagedResponseDto> GetProductList(GetProductList query)
    {
        var result = await mediator.Send(query);
        return new ProductListPagedResponseDto(result.Items.ToList(), result.PageNumber, result.PageSize, result.TotalPages, result.TotalItemsCount);
    }

    public Task<ProductDto> GetProduct(GetProduct query)
        => mediator.Send(query);

    public Task UpdateProduct(UpdateProduct command)
        => mediator.Send(command);

    public Task SoftDeleteProduct(SoftDeleteProduct command)
        => mediator.Send(command);

    public Task HardDeleteProduct(HardDeleteProduct command)
        => mediator.Send(command);

    public Task RecoverProduct(RecoverProduct command)
        => mediator.Send(command);

    public Task HideProduct(HideProduct command)
        => mediator.Send(command);

    public Task RevealProduct(RevealProduct command)
        => mediator.Send(command);

    public async Task<TaxRateListResponseDto> GetTaxRates()
    {
        var result = await mediator.Send(new GetTaxRates());
        return new TaxRateListResponseDto(result.ToList());
    }
}
