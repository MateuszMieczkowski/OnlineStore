using OnlineStore.Server.Infrastructure;

namespace OnlineStore.Server.Features.ShoppingCart.SaveShoppingCart;

public class SaveShoppingCartCommandHandler : ICommandHandler<Shared.ShoppingCart.SaveShoppingCart>
{
    private readonly IShoppingCartCookieService _shoppingCartService;

    public SaveShoppingCartCommandHandler(IShoppingCartCookieService shoppingCartCookieService)
    {
        _shoppingCartService = shoppingCartCookieService ?? throw new ArgumentNullException(nameof(shoppingCartCookieService));
    }
    
    public Task Handle(Shared.ShoppingCart.SaveShoppingCart command, CancellationToken cancellationToken)
    {
        _shoppingCartService.SaveShoppingCart(command.ShoppingCart);
        return Task.CompletedTask;
    }
}