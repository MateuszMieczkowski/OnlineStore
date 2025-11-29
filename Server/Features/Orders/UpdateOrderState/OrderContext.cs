using OnlineStore.Server.Entities;

namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public class OrderContext
{
    public Order Order { get; }

    public OrderAddress OrderAddress { get; }

    public User Client { get; }

    private IOrderState _state;
    
    public OrderContext(Order order, OrderAddress orderAddress, User client, IOrderState state)
    {
        Order = order;
        OrderAddress = orderAddress;
        Client = client;
        _state = state;
    }

    public void SetState(IOrderState state)
    {
        _state = state;
    }

	public async Task CreateAsync()
    {
        await _state.CreateOrderAsync(this);
    }

	public async Task ProcessAsync()
    {
        await _state.ProcessOrderAsync(this);
    }
	public async Task CompleteAsync()
    {
        await _state.CompleteOrderAsync(this);
    }
	public async Task CancelAsync()
    {
        await _state.CancelOrderAsync(this);
    }

}