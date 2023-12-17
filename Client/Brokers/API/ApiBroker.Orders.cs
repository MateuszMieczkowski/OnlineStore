using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Brokers.API;

public partial class ApiBroker
{
    private const string OrdersRelativeUrl = "api/orders";
    
    public async Task<int> CreateOrderAsync(CreateOrder command)
    {
        return await PostAsync<CreateOrder, int>(OrdersRelativeUrl, command);
    }

    public async Task<PagedResult<OrderListItemDto>> GetOrdersAsync(GetOrders query)
    {
        var queryParams = $"?pageNumber={query.PageNumber}&pageSize={query.PageSize}";

        if (query.ClientId != null)
        {
            queryParams += $"&clientId={query.ClientId}";
        }
        
        return await GetAsync<PagedResult<OrderListItemDto>>($"{OrdersRelativeUrl}{queryParams}");
    }

    public async Task<OrderDto> GetOrderAsync(int id)
    {
        var queryUrl = $"{OrdersRelativeUrl}/{id}";
        return await GetAsync<OrderDto>(queryUrl);
    }

    public async Task CompleteOrderAsync(int id)
    {
        var queryUrl = $"{OrdersRelativeUrl}/{id}/complete";
        await PostAsync(queryUrl, null as object);
    }
    
    public async Task CancelOrderAsync(int id)
    {
        var queryUrl = $"{OrdersRelativeUrl}/{id}/cancel";
        await PostAsync(queryUrl, null as object);
    }
    public async Task ProcessOrderAsync(int id)
    {
        var queryUrl = $"{OrdersRelativeUrl}/{id}/process";
        await PostAsync(queryUrl, null as object);
    }
}
