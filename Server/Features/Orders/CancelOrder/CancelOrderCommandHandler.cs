using OnlineStore.Server.Features.Orders.Repository;
using OnlineStore.Server.Features.Orders.UpdateOrderState;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Email;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Features.Orders.CancelOrder;

public class CancelOrderCommandHandler : ICommandHandler<Shared.Orders.CancelOrder>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
    }

    public async Task Handle(Shared.Orders.CancelOrder request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId,
                        includeUser: true,
                        includeOrderItems: false,
                        userId: null,
                        cancellationToken)
                    ?? throw new NotFoundException($"Nie znaleziono zamówienia o ID {request.OrderId}");

        var orderContext = new OrderContext(order, new OrderCanceledState(_emailService));
        await orderContext.CancelAsync();

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}