using OnlineStore.Shared.ShoppingCart;
using System.ServiceModel;

namespace OnlineStore.Shared.SoapContracts;

[ServiceContract]
public interface IShoppingCardSoapService
{
    [OperationContract]
    Task<ShoppingCartDto?> GetShoppingCart();

    [OperationContract]
    Task AddToCart(SaveShoppingCart command);
}
