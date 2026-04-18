﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Shared.Accounts;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Models;
using System.Security.Claims;
using LoginEventDto = OnlineStore.Shared.Accounts.LoginEventDto;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoginEventService _loginEventService;

    public AccountController(IMediator mediator, ILoginEventService loginEventService)
    {
        _mediator = mediator;
        _loginEventService = loginEventService ?? throw new ArgumentNullException(nameof(loginEventService));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PagedResult<UserDto>> GetUsers(int pageNumber , int pageSize)
    {
        var query = new GetUserList(pageNumber, pageSize);
        var response = await _mediator.Send(query);
        return response;
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

    // [HttpGet("login-events")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    // public async Task<ActionResult<LoginEventDetailsDto>> GetLoginEvents(int? limit = null)
    // {
    //     var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (!int.TryParse(userIdClaim, out var userId))
    //     {
    //         return Unauthorized();
    //     }
    //
    //     var history = await _loginEventService.GetLoginHistoryAsync(userId, limit ?? 10);
    //     var dto = new LoginEventDetailsDto(
    //         LoginEvents: history,
    //         LastSuccessfulLoginAt: history.FirstOrDefault(x => x.IsSuccessful)?.EventDate,
    //         LastFailedLoginAt: history.FirstOrDefault(x => !x.IsSuccessful)?.EventDate
    //     );
    //
    //     return Ok(dto);
    // }

    [HttpGet("login-summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginEventDto>> GetLoginSummary()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
    
        var lastSuccess = await _loginEventService.GetLastSuccessfulLoginAsync(userId);
        var lastFailure = await _loginEventService.GetLastFailedLoginAsync(userId);
        var failedCount = await _loginEventService.GetFailedLoginCountSinceLastSuccessAsync(userId);
    
        var summary = new LoginEventDto
        {
            LastSuccessfulLoginAt = lastSuccess?.EventDate,
            LastFailedLoginAt = lastFailure?.EventDate,
            FailedLoginAttemptsSinceLastSuccess = failedCount
        };
    
        return Ok(summary);
    }
}