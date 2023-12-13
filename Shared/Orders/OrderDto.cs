using OnlineStore.Shared.Enums;

namespace OnlineStore.Shared.Orders;

public record OrderDto(
    int Id,
    decimal TotalNet,
    decimal TotalGross,
    OrderStatusDto Status,
    int ClientId,
    DateTime CreatedDate,
    DateTime ModifiedDate,
    OrderAddressDto OrderAddress,
    IReadOnlyCollection<OrderItemDto> Items);

public record OrderItemDto(int Id, decimal PriceNet, decimal PriceGross, int Quantity, int ProductId, string? ProductName = null, string? ProductThumbnailUri = null);