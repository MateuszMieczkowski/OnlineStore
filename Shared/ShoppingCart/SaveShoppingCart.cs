using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.ShoppingCart;

public record SaveShoppingCart(ShoppingCartDto ShoppingCart) : ICommand;