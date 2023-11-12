using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Services;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/size")]
public class SizeController : ControllerBase
{
 

    public SizeController()
    {

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SizeDto>>> GetAll()
    {
        return Ok();
    }

    [HttpPost]
    public ActionResult<SizeDto> Create([FromBody] CreateSizeDto dto)
    {
        return Created("api/size", null);
    }

    [HttpPut("{id}")]
    public ActionResult<SizeDto> Update([FromRoute] Guid id, [FromBody] UpdateSizeDto dto)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Remove([FromRoute] Guid id)
    {
        return NoContent();
    }
}