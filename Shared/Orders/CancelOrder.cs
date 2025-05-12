using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public class CancelOrder : ICommand
{
    public CancelOrder(int orderId)
    {
        OrderId = orderId;
    }

    public CancelOrder() { }
    
    public int OrderId { get; set; }
}