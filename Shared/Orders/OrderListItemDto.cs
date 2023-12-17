using OnlineStore.Shared.Enums;

namespace OnlineStore.Shared.Orders;

public record OrderListItemDto(
    int Id,
    decimal TotalNet,
    decimal TotalGross,
    int ClientId,
    string ClientEmail,
    DateTime CreatedDate)
{
    public DateTime ModifiedDate { get; set; }
    public OrderStatusDto Status { get; set; }
};
