using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;
using System.ServiceModel;

namespace OnlineStore.Shared.SoapContracts;

[ServiceContract]
public interface IProductSoapService
{
    [OperationContract]
    Task CreateProductsBatch(CreateProductsBatch command);

    [OperationContract]
    Task<ProductListPagedResponseDto> GetProductList(GetProductList query);

    [OperationContract]
    Task<ProductDto> GetProduct(GetProduct query);

    [OperationContract]
    Task UpdateProduct(UpdateProduct command);

    [OperationContract]
    Task SoftDeleteProduct(SoftDeleteProduct command);

    [OperationContract]
    Task HardDeleteProduct(HardDeleteProduct command);

    [OperationContract]
    Task RecoverProduct(RecoverProduct command);

    [OperationContract]
    Task HideProduct(HideProduct command);

    [OperationContract]
    Task RevealProduct(RevealProduct command);

    [OperationContract]
    Task<TaxRateListResponseDto> GetTaxRates();
}
