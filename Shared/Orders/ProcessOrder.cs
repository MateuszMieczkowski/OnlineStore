using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public class ProcessOrder : ICommand
{
    public ProcessOrder(int orderId)
    {
        OrderId = orderId;
    }

    public ProcessOrder() { }
    
    public int OrderId { get; set; }
}