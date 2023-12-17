namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public interface IOrderState
{
    Task CreateOrderAsync(OrderContext context);
    Task ProcessOrderAsync(OrderContext context);
    Task CancelOrderAsync(OrderContext context);
    Task CompleteOrderAsync(OrderContext context);
}