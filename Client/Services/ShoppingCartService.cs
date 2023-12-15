using Blazored.LocalStorage;
using OnlineStore.Client.Models.Orders;

namespace OnlineStore.Client.Services;

public interface IShoppingCartService
{
    Task<CartModel> GetCart();
    
    Task AddToCartAsync(CartItemModel product, int count);
    
    Task RemoveFromCart(int productId);
    
    Task ClearCart();

    Task Reorder(int orderId);
}

public class ShoppingCartService : IShoppingCartService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IOrderService _orderService;

    public ShoppingCartService(ILocalStorageService localStorageService, IOrderService orderService)
    {
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public async Task<CartModel> GetCart()
        => await _localStorageService.GetItemAsync<CartModel>(LocalStorageKeys.Cart) ?? new CartModel();
    
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

    public async Task ClearCart()
    {
        await _localStorageService.RemoveItemAsync(LocalStorageKeys.Cart);
    }

    public async Task Reorder(int orderId)
    {
        var order = await _orderService.GetOrder(orderId);
        var cartItems = order.Items.Select(x => new CartItemModel
        {
            Count = x.Quantity,
            Name = x.ProductName ?? string.Empty,
            ThumbnailUri = x.ProductThumbnailUri ?? string.Empty,
            ProductId = x.ProductId
        }).ToList();
        
        var cart = new CartModel { Items = cartItems };
        await SetCart(cart);
    }

    private async Task SetCart(CartModel cart)
        => await _localStorageService.SetItemAsync(LocalStorageKeys.Cart, cart);
}