using Blazored.LocalStorage;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Models.Orders;
using OnlineStore.Client.Services;
using OnlineStore.Shared.ShoppingCart;
using OnlineStore.Shared.SoapContracts;

namespace OnlineStore.Client.SoapServices;

public class ShoppingCardSoapService( 
    ILocalStorageService localStorageService, 
    IOrderService orderService,
    ISoapClient soapClient)
    : IShoppingCartService
{
    private const string Endpoint = "/soap/shopping-card";
    private const string ServiceNamespace = "http://tempuri.org/IShoppingCardService";
    
    public async Task<CartModel> GetCartFromStorage() => await localStorageService.GetItemAsync<CartModel>(LocalStorageKeys.Cart) ?? new CartModel();
    
    public async Task LoadCartFromServer()
    {
        // var cartDto = await apiBroker.GetShoppingCartFromCache();
        var cartDto = await soapClient.SendQuery<GetShoppingCart, ShoppingCartDto?>(
            endpoint: Endpoint, 
            serviceNamespace: ServiceNamespace,
            actionName: nameof(GetShoppingCart), 
            payload: null);

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

        await soapClient.SendCommand(Endpoint, ServiceNamespace, "AddToCart", payload: command);
        // await apiBroker.SaveShoppingCartToCache(command);
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

    public async Task ClearCart() => await localStorageService.RemoveItemAsync(LocalStorageKeys.Cart);

    public async Task Reorder(int orderId)
    {
        var order = await orderService.GetOrder(orderId);
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

    private async Task SetCart(CartModel cart) => await localStorageService.SetItemAsync(LocalStorageKeys.Cart, cart);
}
