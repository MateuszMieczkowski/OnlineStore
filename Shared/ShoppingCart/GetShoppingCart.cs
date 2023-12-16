using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.ShoppingCart;

public record GetShoppingCart : IQuery<ShoppingCartDto?>;