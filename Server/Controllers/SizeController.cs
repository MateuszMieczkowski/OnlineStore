using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Services;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/size")]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SizeDto>>> GetAll() => Ok(await _sizeService.GetAll());

        [HttpPost]
        public ActionResult<SizeDto> Create([FromBody] CreateSizeDto dto)
        {
            return Created("api/size", _sizeService.Create(dto));
        }

        [HttpPut("{id}")]
        public ActionResult<SizeDto> Update([FromRoute] Guid id, [FromBody] UpdateSizeDto dto)
        {
            return Ok(_sizeService.Update(id, dto));
        }
        [HttpDelete("{id}")]
        public ActionResult Remove([FromRoute] Guid id)
        {
            _sizeService.Remove(id);
            return NoContent();
        }
    }
}
