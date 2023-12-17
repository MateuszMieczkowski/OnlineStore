
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
		order.Status = OrderStatus.Canceled;

		var emailDefinition = new OrderStatusChangedEmail(
				order: order,
				recipientEmail: order.User.Email,
				recipientName: order.User.FullName,
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
