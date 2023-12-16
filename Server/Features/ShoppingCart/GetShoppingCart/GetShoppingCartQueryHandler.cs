using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Server.Features.ShoppingCart.GetShoppingCart;

public class GetShoppingCartQueryHandler : IQueryHandler<Shared.ShoppingCart.GetShoppingCart, ShoppingCartDto?>
{
    private readonly IShoppingCartCookieService _shoppingCartService;

    public GetShoppingCartQueryHandler(IShoppingCartCookieService shoppingCartCookieService)
    {
        _shoppingCartService = shoppingCartCookieService ?? throw new ArgumentNullException(nameof(shoppingCartCookieService));
    }
    
    public Task<ShoppingCartDto?> Handle(Shared.ShoppingCart.GetShoppingCart query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_shoppingCartService.GetShoppingCart());
    }
}