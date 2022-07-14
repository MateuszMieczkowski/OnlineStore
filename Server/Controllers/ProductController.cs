using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SneakerBase.Shared.Dtos;
using SneakersBase.Server.Services;
using SneakerBase.Entities;
using SneakersBase.Shared.Dtos;

namespace SneakerBase.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAll() => Ok(_productService.GetAll());

        [HttpGet("/api/products/search")]
        public ActionResult<IEnumerable<ProductDto>> GetBySerach([FromQuery] string filter) =>
            Ok(_productService.GetBySerach(filter));

        [HttpPost]
        public ActionResult PostMany([FromBody] List<CreateProductDto> dtos)
        {
            _productService.CreateMany(dtos);
            return Created("api/products", null);
        }

        [HttpDelete("/api/products/{id}")]
        public ActionResult RemoveById([FromRoute] int id)
        {
            _productService.RemoveById(id);
            return NoContent();
        }

        [HttpPut("/api/products/{id}")]
        public ActionResult<ProductDto> Update([FromRoute] int id, [FromBody] UpdateProductDto dto)
        {
            return Ok(_productService.Update(id, dto));
        }

}
}
