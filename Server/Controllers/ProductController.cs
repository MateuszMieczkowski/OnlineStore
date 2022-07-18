using Microsoft.AspNetCore.Mvc;
using SneakersBase.Server.Services;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server.Controllers
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

        [HttpGet("search")]
        public ActionResult<IEnumerable<ProductDto>> GetBySerach([FromQuery] string filter) =>
            Ok(_productService.GetBySerach(filter));

        [HttpPost]
        public ActionResult PostMany([FromBody] List<CreateProductDto> dtos)
        {
            _productService.CreateMany(dtos);
            return Created("api/products", null);
        }

        [HttpDelete("{id:int}")]
        public ActionResult RemoveById([FromRoute] int id)
        {
            _productService.RemoveById(id);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDto> Update([FromRoute] int id, [FromBody] UpdateProductDto dto)
        {
            return Ok(_productService.Update(id, dto));
        }

}
}
