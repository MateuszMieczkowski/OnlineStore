using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string OrdersRelativeUrl = "api/orders";
    
    public async Task CreateOrderAsync(CreateOrder command)
    {
        await PostAsync(OrdersRelativeUrl, command);
    }

    public async Task<PagedResult<OrderListItemDto>> GetOrdersAsync(GetOrders query)
    {
        var queryParams = $"?pageNumber={query.PageNumber}&pageSize={query.PageSize}";
        
        return await GetAsync<PagedResult<OrderListItemDto>>($"{OrdersRelativeUrl}{queryParams}");
    }
}
