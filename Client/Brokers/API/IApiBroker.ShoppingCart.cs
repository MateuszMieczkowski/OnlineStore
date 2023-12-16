using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<ShoppingCartDto?> GetShoppingCartFromCache();

    Task SaveShoppingCartToCache(SaveShoppingCart command);
}