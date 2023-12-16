using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Server.Features.ShoppingCart;

public interface IShoppingCartCookieService
{
    ShoppingCartDto? GetShoppingCart();
    void SaveShoppingCart(ShoppingCartDto shoppingCart);
}


public class ShoppingCartService : IShoppingCartCookieService
{
    private readonly ILoggedUserService _loggedUserService;
    private readonly IMemoryCache _memoryCache;

    public ShoppingCartService(ILoggedUserService loggedUserService, IMemoryCache memoryCache)
    {
        _loggedUserService = loggedUserService ?? throw new ArgumentNullException(nameof(loggedUserService));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public ShoppingCartDto? GetShoppingCart()
    {
        var userId = _loggedUserService.GetUserId();
        var cacheKey = CacheKeys.GetShoppingCartKey(userId);

        if (_memoryCache.TryGetValue<string?>(cacheKey, out var cartData)) ;

        if (cartData == null)
        {
            return null;
        }

        var shoppingCart = JsonSerializer.Deserialize<ShoppingCartDto>(cartData);
        return shoppingCart;
    }
    
    public void SaveShoppingCart(ShoppingCartDto shoppingCart)
    {
        var userId = _loggedUserService.GetUserId();
        var cacheKey = CacheKeys.GetShoppingCartKey(userId);

        var serializedCart = JsonSerializer.Serialize(shoppingCart);
        _memoryCache.Set(cacheKey, serializedCart, TimeSpan.FromMinutes(30));
    }
}