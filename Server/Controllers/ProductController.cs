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
        private readonly IBlobStorage _azureStorage;

        public ProductController(IProductService productService, IBlobStorage azureStorage)
        {
            _productService = productService;
            _azureStorage = azureStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll() => Ok(await _productService.GetAllAsync());

        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllTest() => Ok(await _productService.GetAllAsync());

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetBySerach([FromQuery] string filter) =>
            Ok(await _productService.GetBySerachAsync(filter));

        [HttpPost]
        public async Task<ActionResult> PostMany([FromBody] List<CreateProductDto> dtos)
        {

            var products = await _productService.CreateManyAsync(dtos);
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
            await _azureStorage.Update(id, dto);
            var updatedDto = await _productService.UpdateAsync(id, dto);

            return Ok(updatedDto);
        }

    }
}
