
using OnlineStore.Server.Emails.EmailDefinitions;
using OnlineStore.Server.Enums;
using OnlineStore.Server.Services.Email;

namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public class OrderCompletedState : IOrderState
{
	private readonly IEmailService _emailService;

	public OrderCompletedState(IEmailService emailService)
	{
		_emailService = emailService;
	}

	public Task CancelOrderAsync(OrderContext context)
	{
		return Task.CompletedTask;
	}

	public async Task CompleteOrderAsync(OrderContext context)
	{
		var order = context.Order;
		if(order.Status == OrderStatus.Completed)
		{
			return;
		}
		
		order.Status = OrderStatus.Completed;

		var emailDefinition = new OrderStatusChangedEmail(
				order: order,
				recipientEmail: order.Client.Email,
				recipientName: order.Client.FullName,
				senderEmail: null);

		await _emailService.SendEmailFromDefinitionAsync(emailDefinition);
	}

	public Task CreateOrderAsync(OrderContext context)
	{
		return Task.CompletedTask;
	}

	public Task ProcessOrderAsync(OrderContext context)
	{
		return Task.CompletedTask;
	}
}
