using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PagedResult<UserDto>> GetUsers(int pageNumber , int pageSize)
    {
        var query = new GetUserList(pageNumber, pageSize);
        var reponse = await _mediator.Send(query);
        return reponse;
    }
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<CreatedResult> RegisterUser([FromBody] RegisterAdmin command)
    {
        await _mediator.Send(command);
        return Created("/users", null);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<AuthResponse> Login([FromBody] AuthenticateUser request)
    {
        var response = await _mediator.Send(request);
        return response;
    }
    
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> ChangePassword(ChangeUserPassword command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> ForgotPassword(ForgotPassword command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> ResetPassword(ResetPassword command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}