﻿using OnlineStore.Server.Features.Orders.Repository;
using OnlineStore.Server.Features.Orders.UpdateOrderState;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Server.Services.Email;
using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Features.Orders.CompleteOrder;

public class CompleteOrderCommandHandler : ICommandHandler<Shared.Orders.CompleteOrder>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;

    public CompleteOrderCommandHandler(IEmailService emailService, IOrderRepository orderRepository)
    {
        _emailService = emailService;
        _orderRepository = orderRepository;
    }

    public async Task Handle(Shared.Orders.CompleteOrder request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId,
                        includeUser: true,
                        includeOrderItems: false,
                        userId: null,
                        cancellationToken)
                    ?? throw new NotFoundException($"Nie znaleziono zamówienia o ID {request.OrderId}");

        var orderContext = new OrderContext(order, new OrderCompletedState(_emailService));
        await orderContext.CompleteAsync();

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}