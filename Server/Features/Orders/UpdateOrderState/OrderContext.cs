using OnlineStore.Server.Entities;

namespace OnlineStore.Server.Features.Orders.UpdateOrderState;

public class OrderContext
{
    public Order Order { get; }
    private IOrderState _state;
    
    public OrderContext(Order order, IOrderState state)
    {
        Order = order;
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