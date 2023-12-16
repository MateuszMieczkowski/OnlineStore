using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Server.Features.ShoppingCart.GetShoppingCart;

public class GetShoppingCartQueryHandler : IQueryHandler<Shared.ShoppingCart.GetShoppingCart, ShoppingCartDto?>
{
    private readonly IShoppingCartCookieService _shoppingCartCookieService;

    public GetShoppingCartQueryHandler(IShoppingCartCookieService shoppingCartCookieService)
    {
        _shoppingCartCookieService = shoppingCartCookieService ?? throw new ArgumentNullException(nameof(shoppingCartCookieService));
    }
    
    public Task<ShoppingCartDto?> Handle(Shared.ShoppingCart.GetShoppingCart query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_shoppingCartCookieService.GetShoppingCart());
    }
}