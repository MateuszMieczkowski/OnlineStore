using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public class GetOrder : IQuery<OrderDto>
{
    public int Id { get; set; }

    public GetOrder(int id)
    {
        Id = id;
    }

    public GetOrder() { }
}

