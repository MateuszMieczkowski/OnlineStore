using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.Authentication;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Server.Controllers
{
	[Route("api/orders")]
	[Authorize]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IMediator _mediator;

		public OrderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult> CreateOrder([FromBody] CreateOrder command)
		{
			await _mediator.Send(command);

			return Created("/orders", command.CreatedId);
		}

		[HttpGet]
		public async Task<PagedResult<OrderListItemDto>> GetOrders([FromQuery] GetOrders query)
		{
			return await _mediator.Send(query);
		}

		[HttpGet("{id:int}")]
		public async Task<OrderDto> GetOrder([FromRoute] int id)
		{
			return await _mediator.Send(new GetOrder(id));
		}
	}
}
