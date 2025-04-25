
using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Services.Email;

namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public class OrderProcessedState : IOrderState
{
	private readonly IEmailService _emailService;

	public OrderProcessedState(IEmailService emailService)
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

	public Task CreateOrderAsync(OrderContext context)
	{
		return Task.CompletedTask;
	}

	public async Task ProcessOrderAsync(OrderContext context)
	{
		var order = context.Order;
		if(order.Status == OrderStatus.Processing)
		{
			return;
		}
		
		order.Status = OrderStatus.Processing;

		var emailDefinition = new OrderStatusChangedEmail(
			order: order,
			recipientEmail: order.Client.Email,
			recipientName: order.Client.FullName,
			senderEmail: null);

		await _emailService.SendEmailFromDefinitionAsync(emailDefinition);
	}
}
