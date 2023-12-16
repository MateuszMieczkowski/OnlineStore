using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.ShoppingCart.SaveShoppingCart;

public class SaveShoppingCartCommandHandler : ICommandHandler<Shared.ShoppingCart.SaveShoppingCart>
{
    private readonly IShoppingCartCookieService _shoppingCartCookieService;

    public SaveShoppingCartCommandHandler(IShoppingCartCookieService shoppingCartCookieService)
    {
        _shoppingCartCookieService = shoppingCartCookieService ?? throw new ArgumentNullException(nameof(shoppingCartCookieService));
    }
    
    public Task Handle(Shared.ShoppingCart.SaveShoppingCart command, CancellationToken cancellationToken)
    {
        _shoppingCartCookieService.SaveShoppingCart(command.ShoppingCart);
        return Task.CompletedTask;
    }
}