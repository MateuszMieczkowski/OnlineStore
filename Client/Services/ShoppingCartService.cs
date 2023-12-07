using Blazored.LocalStorage;
using OnlineStore.Client.Models.Orders;
using OnlineStore.Shared.Orders;
using OnlineStore.Shared.Products;

namespace OnlineStore.Client.Services;

public interface IShoppingCartService
{
    
    Task<CartModel> GetCart();
    
    Task AddToCartAsync(ProductListItemDto product, int count);
    
    Task AddToCartAsync(CartItemModel product, int count);
    
    Task RemoveFromCart(int productId);
}

public class ShoppingCartService : IShoppingCartService
{
    private readonly ILocalStorageService _localStorageService;

    public ShoppingCartService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
    }

    public async Task<CartModel> GetCart()
        => await _localStorageService.GetItemAsync<CartModel>(LocalStorageKeys.Cart) ?? new CartModel();
    

    public async Task AddToCartAsync(ProductListItemDto product, int count)
    {
        var carItem = new CartItemModel
        {
            ProductId = product.Id,
            Name = product.Name,
            ThumbnailUri = product.ThumbnailUri,
            Count = count
        };

        await AddToCartAsync(carItem, count);
    }

    public async Task AddToCartAsync(CartItemModel product, int count)
    {
        var cart = await GetCart();

        var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == product.ProductId);
        
        if (cartItem != null)
        {
            cartItem.Count += count;
        }
        else
        {
            cart.Items.Add(product);
        }
        
        await SetCart(cart);
    }

    public async Task RemoveFromCart(int productId)
    {
        var cart = await GetCart();
        var cartItem = cart.Items.FirstOrDefault(x => x.ProductId == productId);
        if (cartItem is null)
        {
            return;
        }

        cart.Items.Remove(cartItem);
        await SetCart(cart);
    }
    
    private async Task SetCart(CartModel cart)
        => await _localStorageService.SetItemAsync(LocalStorageKeys.Cart, cart);
}