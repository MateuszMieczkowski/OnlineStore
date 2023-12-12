using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Models.Orders;
using OnlineStore.Shared.Infrastructure;
using OnlineStore.Shared.Orders;

namespace OnlineStore.Client.Services;

public interface IOrderService
{
    Task<int> CreateOrder(CartModel cartModel);
    
    Task<PagedResult<OrderListItemDto>> GetOrders(int pageNumber, int pageSize, int? clientId);
    
    Task<OrderDto> GetOrder(int id);
}

public class OrderService : IOrderService
{
    private readonly IApiBroker _broker;

    public OrderService(IApiBroker broker)
    {
        _broker = broker;
    }
    
    public async Task<int> CreateOrder(CartModel cartModel)
    {
        var orderItems = cartModel.Items.Select(x => new CreateOrder.CreateOrderItem(x.ProductId, x.Count)).ToList();
        var command = new CreateOrder(cartModel.AddressId, orderItems);
        return await _broker.CreateOrderAsync(command);
    }

    public async Task<PagedResult<OrderListItemDto>> GetOrders(int pageNumber, int pageSize, int? clientId)
    {
        return await _broker.GetOrdersAsync(new GetOrders(pageNumber, pageSize, null, clientId));
    }

    public async Task<OrderDto> GetOrder(int id)
    {
        return await _broker.GetOrderAsync(id);
    }
}
