using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<CreatedResult> RegisterUser([FromBody] RegisterUser command)
    {
        await _mediator.Send(command);
        return Created("/users", null);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<AuthResponse> Login([FromBody] AuthenticateUser request)
    {
        var response = await _mediator.Send(request);
        return response;
    }
}