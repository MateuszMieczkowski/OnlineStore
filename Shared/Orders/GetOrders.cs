using OnlineStore.Shared.Infrastructure;

namespace OnlineStore.Shared.Orders;

public class GetOrders : IPagedQuery<OrderListItemDto>
{
    public GetOrders(int pageNumber, int pageSize, string? orderStatuses, int? clientId)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        OrderStatuses = orderStatuses;
        ClientId = clientId;
    }

    public GetOrders() { }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public string? OrderStatuses { get; set; }

    public int? ClientId { get; set; }
}