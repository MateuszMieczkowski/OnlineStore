using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Services;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<AuthResponse> Login([FromBody] LoginDto dto)
        {
            var authResponse = _accountService.Login(dto);
            return Ok(authResponse);
        }
    }
}
