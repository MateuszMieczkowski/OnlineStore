using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public class CompleteOrder : ICommand
{
    public CompleteOrder(int orderId)
    {
        OrderId = orderId;
    }
    
    public CompleteOrder() { }

    public int OrderId { get; set; }
}