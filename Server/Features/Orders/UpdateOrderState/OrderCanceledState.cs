using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Entities;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Services.Email;

namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public class OrderCanceledState : IOrderState
{
	private readonly IEmailService _emailService;

	public OrderCanceledState(IEmailService emailService)
	{
		_emailService = emailService;
	}

	public async Task CancelOrderAsync(OrderContext context)
	{
		var order = context.Order;
		order.Status = OrderStatus.Canceled;

		var emailDefinition = new OrderStatusChangedEmail(
				order: order,
				recipientEmail: order.User.Email,
				recipientName: order.User.FullName,
				senderEmail: null);

		await _emailService.SendEmailFromDefinitionAsync(emailDefinition);
	}

	public Task CompleteOrderAsync(OrderContext context)
	{
		return Task.CompletedTask;
	}

	public Task CreateOrderAsync(OrderContext context)
	{
		return Task.CompletedTask;
	}

	public async Task ProcessOrderAsync(OrderContext context)
	{
		context.SetState(new OrderProcessedState(_emailService));
		await context.ProcessAsync();
	}
}
