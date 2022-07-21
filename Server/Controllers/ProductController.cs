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
        public async Task<ActionResult> PostMany([FromBody] List<CreateProductDto> dtos)
        {

            var products = _productService.CreateMany(dtos);
            await _azureStorage.Upload(dtos, products);

            return Created("api/products", null);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveById([FromRoute] int id)
        {
            _productService.RemoveById(id);
            await _azureStorage.Remove(id);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> Update([FromRoute] int id, [FromBody] UpdateProductDto dto)
        {
            _productService.Update(id, dto);
            await _azureStorage.Update(id, dto);
            return Ok();
        }

    }
}
