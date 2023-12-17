using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Brokers.API;

public partial interface IApiBroker
{
    Task<int> CreateOrderAsync(CreateOrder command);

    Task<PagedResult<OrderListItemDto>> GetOrdersAsync(GetOrders query);
    
    Task<OrderDto> GetOrderAsync(int id);
    
    Task CompleteOrderAsync(int id);
    
    Task CancelOrderAsync(int id);
    
    Task ProcessOrderAsync(int id);
}
