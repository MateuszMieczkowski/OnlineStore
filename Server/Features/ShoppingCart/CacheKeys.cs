namespace OnlineStore.Server.Features.ShoppingCart;

public class CacheKeys
{
    public const string ShoppingCart = "ShoppingCart";

    public static string GetShoppingCartKey(int userId) => $"{ShoppingCart}-{userId}";
}