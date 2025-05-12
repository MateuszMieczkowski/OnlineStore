using OnlineStore.Shared.Infrastructure;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Orders;

[DataContract]
public class GetAllOrders : IPagedQuery<OrderListItemDto>
{
    public GetAllOrders(int pageNumber, int pageSize, int? userId, string? orderStatuses)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        UserId = userId;
        OrderStatuses = orderStatuses;
    }

    public GetAllOrders() { }
    
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int? UserId { get; set; }

    public string? OrderStatuses { get; set; }
}