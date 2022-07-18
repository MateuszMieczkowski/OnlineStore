using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SneakersBase.Server.Services;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server.Controllers
{
    [ApiController]
    [Route("api/size")]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService) 
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SizeDto>> GetAll() => Ok(_sizeService.GetAll());

        [HttpPost]
        public ActionResult<SizeDto> Create([FromBody] CreateSizeDto dto)
        {
            return Created("api/size", _sizeService.Create(dto));
        }

        [HttpPut("{id:int}")]
        public ActionResult<SizeDto> Update([FromRoute] int id, [FromBody] UpdateSizeDto dto)
        {
            return Created("api/size", _sizeService.Update(id, dto));
        }
        [HttpDelete("{id:int}")]
        public ActionResult Remove([FromRoute] int id)
        {
            _sizeService.Remove(id);
            return NoContent();
        }
    }
}
