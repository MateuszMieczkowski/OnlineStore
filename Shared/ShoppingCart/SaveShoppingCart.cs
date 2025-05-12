using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.ShoppingCart;

public class SaveShoppingCart : ICommand
{
    public SaveShoppingCart(ShoppingCartDto shoppingCart)
    {
        ShoppingCart = shoppingCart;
    }

    public SaveShoppingCart() { }

    public ShoppingCartDto ShoppingCart { get; set; }
}