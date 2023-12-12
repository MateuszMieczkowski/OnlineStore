using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Shared.Clients;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<CreatedResult> RegisterUser([FromBody] RegisterClient command)
    {
        await _mediator.Send(command);
        return Created("/clients", null);
    }    
    
    [HttpPut("change-user-preferences")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> ChangePassword(ChangeUserPreferences command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("order-address")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<OrderAddressDto?> GetOrderAddress()
    {
        var query = new GetOrderAddress();
        var response = await _mediator.Send(query);
        return response;
    }
    
    [HttpPut("order-address")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> UpsertOrderAddress(UpsertOrderAddress command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
