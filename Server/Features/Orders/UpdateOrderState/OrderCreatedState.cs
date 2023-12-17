using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Services.Email;

namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public class OrderCreatedState : IOrderState
{
    private readonly IEmailService _emailService;
	public OrderCreatedState(IEmailService emailService)
	{
		_emailService = emailService;
	}

	public async Task CancelOrderAsync(OrderContext context)
	{
		context.SetState(new OrderCanceledState(_emailService));
		await context.CancelAsync();
	}

	public async Task CompleteOrderAsync(OrderContext context)
	{
		context.SetState(new OrderCompletedState(_emailService));
		await context.CompleteAsync();
	}

	public async Task CreateOrderAsync(OrderContext context)
	{
		var order = context.Order;
		if(order.Status == OrderStatus.Created)
		{
			return;
		}
		var client = order.User;

		order.Status = OrderStatus.Created;

		var orderCreatedEmail = new OrderCreatedSummaryEmail(
			order: order,
		recipientEmail: client.Email,
		recipientName: client.FullName,
			senderEmail: null);

		await _emailService.SendEmailFromDefinitionAsync(orderCreatedEmail);
	}

	public async Task ProcessOrderAsync(OrderContext context)
	{
		context.SetState(new OrderProcessedState(_emailService));
		await context.ProcessAsync();
	}
}