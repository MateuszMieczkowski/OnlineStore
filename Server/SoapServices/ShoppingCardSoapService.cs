using MediatR;
using OnlineStore.Shared.ShoppingCart;
using OnlineStore.Shared.SoapContracts;

namespace OnlineStore.Server.SoapServices;

public class ShoppingCardSoapService : IShoppingCardSoapService
{
    private readonly IMediator _mediator;

    public ShoppingCardSoapService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<ShoppingCartDto?> GetShoppingCart()
    {
        var query = new GetShoppingCart();

        var response = await _mediator.Send(query);
        return response;
    }

    public async Task AddToCart(SaveShoppingCart command)
        => await _mediator.Send(command);
}
