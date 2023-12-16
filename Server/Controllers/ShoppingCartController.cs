using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Shared.ShoppingCart;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/shopping-cart")]
public class ShoppingCartController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShoppingCartController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ShoppingCartDto?> GetShoppingCart()
    {
        var query = new GetShoppingCart();

        var response = await _mediator.Send(query);
        return response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddToCart(SaveShoppingCart command)
    {
        await _mediator.Send(command);
        return Created("/shopping-cart", null);
    }
}