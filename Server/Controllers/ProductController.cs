using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersBase.Server.Entities;
using SneakersBase.Server.Services;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAzureStorage _azureStorage;

        public ProductController(IProductService productService, IAzureStorage azureStorage)
        {
            _productService = productService;
            _azureStorage = azureStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ProductDto>> GetAll() => Ok(_productService.GetAll());

        [HttpGet("search")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ProductDto>> GetBySerach([FromQuery] string filter) =>
            Ok(_productService.GetBySerach(filter));

        [HttpPost]
        public ActionResult PostMany([FromBody] List<CreateProductDto> dtos)
        {
            var products = _productService.CreateMany(dtos);
            _azureStorage.Upload(dtos);
            
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
