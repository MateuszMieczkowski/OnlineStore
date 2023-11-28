using OnlineStore.Shared.Enums;

namespace OnlineStore.Shared.Orders;

public record OrderListItemDto(
    int Id,
    decimal TotalNet,
    decimal TotalGorss,
    OrderStatusDto Status,
    int ClientId,
    DateTime CreatedDate,
    DateTime ModifiedDate);
