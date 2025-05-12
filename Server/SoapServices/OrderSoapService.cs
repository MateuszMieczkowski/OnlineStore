using MediatR;
using OnlineStore.Shared.Orders;
using OnlineStore.Shared.SoapContracts;

namespace OnlineStore.Server.SoapServices;

public class OrderSoapService(IMediator mediator) : IOrderSoapService
{
    public async Task<int> CreateOrder(CreateOrder command)
    {
        await mediator.Send(command);
        return command.CreatedId;
    }

    public async Task<GetAllOrdersResponseDto> GetOrders(GetOrders query)
    {
        var result = await mediator.Send(query);
        return new GetAllOrdersResponseDto(result.Items.ToList(), result.PageNumber, result.PageSize, result.TotalPages, result.TotalItemsCount);
    }

    public Task<OrderDto> GetOrder(GetOrder query)
        => mediator.Send(query);

    public Task CompleteOrder(CompleteOrder command)
        => mediator.Send(command);

    public Task CancelOrder(CancelOrder command)
        => mediator.Send(command);

    public Task ProcessOrder(ProcessOrder command)
        => mediator.Send(command);
}