using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SneakerBase.Shared.Dtos;
using SneakersBase.Server.Services;

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

    }
}
