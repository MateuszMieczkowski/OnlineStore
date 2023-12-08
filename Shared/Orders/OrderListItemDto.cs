using OnlineStore.Shared.Enums;

namespace OnlineStore.Shared.Orders;

public record OrderListItemDto(
    int Id,
    decimal TotalNet,
    decimal TotalGross,
    OrderStatusDto Status,
    int ClientId,
    DateTime CreatedDate,
    DateTime ModifiedDate)
{
    public OrderStatusDto Status { get; set; }
};
