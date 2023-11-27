using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Authentication;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-batch")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateProductsBatch([FromBody] CreateProductsBatch command)
    {
        await _mediator.Send(command);
        return Created("/products", null);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PagedResult<ProductListItemDto>> GetProductList([FromQuery] GetProductList query)
    {
        var response = await _mediator.Send(query);
        return response;
    }
    
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ProductDto> GetProduct([FromRoute] int id,
        [FromQuery] bool includeDeleted = false,
        [FromQuery] bool includeHidden = false)
    {
        var response = await _mediator.Send(new GetProduct(id, includeDeleted, includeHidden));
        return response;
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto)
    {
        await _mediator.Send(updateProductDto.ToCommand(id));
        return Ok();
    }
    
    [HttpDelete("{id:int}/soft-delete")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SoftDeleteProduct([FromRoute] int id)
    {
        await _mediator.Send(new SoftDeleteProduct(id));
        return Ok();
    }
    
    [HttpDelete("{id:int}/hard-delete")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> HardDeleteProduct([FromRoute] int id)
    {
        await _mediator.Send(new HardDeleteProduct(id));
        return Ok();
    }
    
    [HttpPut("{id:int}/recover")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> RecoverProduct([FromRoute] int id)
    {
        await _mediator.Send(new RecoverProduct(id));
        return Ok();
    }
    
    [HttpPut("{id:int}/hide")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> HideProduct([FromRoute] int id)
    {
        await _mediator.Send(new HideProduct(id));
        return Ok();
    }
    
    [HttpPut("{id:int}/reveal")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> RevealProduct([FromRoute] int id)
    {
        await _mediator.Send(new RevealProduct(id));
        return Ok();
    }
    
    [HttpGet("taxRates")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IEnumerable<TaxRateDto>> GetTaxRates()
    {
        var response = await _mediator.Send(new GetTaxRates());
        return response;
    }
}