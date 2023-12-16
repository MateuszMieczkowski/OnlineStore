using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string ShoppingCartRelativeUrl = "api/shopping-cart";
    
    public async Task<ShoppingCartDto?> GetShoppingCartFromCache() => await GetAsync<ShoppingCartDto?>($"{ShoppingCartRelativeUrl}");
    
    public async Task SaveShoppingCartToCache(SaveShoppingCart command) => await PostAsync<SaveShoppingCart?>($"{ShoppingCartRelativeUrl}", command);
}