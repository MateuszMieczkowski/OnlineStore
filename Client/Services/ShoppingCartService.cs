using Blazored.LocalStorage;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Models.Orders;
using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Client.Services;

public interface IShoppingCartService
{
    Task<CartModel> GetCartFromStorage();
    
    Task LoadCartFromServer();
    
    Task SaveCartToServer();
    
    Task AddToCartAsync(CartItemModel product, int count);
    
    Task RemoveFromCart(int productId);
    
    Task ClearCart();

    Task Reorder(int orderId);
}

public class ShoppingCartService : IShoppingCartService
{
    private readonly IApiBroker _apiBroker;
    private readonly ILocalStorageService _localStorageService;
    private readonly IOrderService _orderService;

    public ShoppingCartService(IApiBroker apiBroker, ILocalStorageService localStorageService, IOrderService orderService)
    {
        _apiBroker = apiBroker ?? throw new ArgumentNullException(nameof(apiBroker));
        _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public async Task<CartModel> GetCartFromStorage() => await _localStorageService.GetItemAsync<CartModel>(LocalStorageKeys.Cart) ?? new CartModel();
    
    public async Task LoadCartFromServer()
    {
        var cartDto = await _apiBroker.GetShoppingCartFromCache();
        if (cartDto is null)
        {
            return;
        }
        
        var cartItems = cartDto.Items.Select(x => new CartItemModel
        {
            Count = x.Count,
            Name = x.Name,
            ThumbnailUri = x.ThumbnailUri,
            ProductId = x.ProductId
        }).ToList();
        
        var cart = new CartModel { Items = cartItems };
        await SetCart(cart);
    }

    public async Task SaveCartToServer()
    {
        var cartModel = await GetCartFromStorage();
        var cartItemsDto = cartModel.Items.Select(x => new ShoppingCartItemDto(x.ProductId, x.Name, x.ThumbnailUri, x.Count)).ToList();
        var shoppingCartDto = new ShoppingCartDto(cartItemsDto);
        var command = new SaveShoppingCart(shoppingCartDto);
        await _apiBroker.SaveShoppingCartToCache(command);
    }

    public async Task AddToCartAsync(CartItemModel product, int count)
    {
        var cart = await GetCartFromStorage();

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
        var cart = await GetCartFromStorage();
        var cartItem = cart.Items.FirstOrDefault(x => x.ProductId == productId);
        if (cartItem is null)
        {
            return;
        }

        cart.Items.Remove(cartItem);
        await SetCart(cart);
    }

    public async Task ClearCart() => await _localStorageService.RemoveItemAsync(LocalStorageKeys.Cart);

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

    private async Task SetCart(CartModel cart) => await _localStorageService.SetItemAsync(LocalStorageKeys.Cart, cart);
}