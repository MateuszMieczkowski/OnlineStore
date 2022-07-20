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
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
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

            SaveFiles(products, dtos);
            return Created("api/products", null);
        }
        private async void SaveFiles(List<Product> products, List<CreateProductDto> dtos)
        {
            var rootPath = Directory.GetCurrentDirectory();

            foreach (var product in products)
            {
                var em = dtos.GetEnumerator();
                em.MoveNext();
                var dto = em.Current;

                var filePath = $"{_env.ContentRootPath}/Sneakers/{product.Id}";
                //if (System.IO.File.Exists(fullPath))
                //{
                //    System.IO.File.Delete(fullPath);
                //    ViewBag.deleteSuccess = "true";
                //}
                var buf = Convert.FromBase64String(dto.ThumbnailPath);

                await System.IO.File.WriteAllBytesAsync(filePath, buf);
                   // await Request.Body.CopyToAsync(writer);
            }
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
