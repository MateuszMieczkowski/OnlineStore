using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Shared.Clients;

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
}
