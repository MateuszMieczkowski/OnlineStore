using OnlineStore.Shared.Enums;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Orders;

[DataContract]
[XmlRoot("GetOrderResult")]
public class OrderDto
{
    public OrderDto(
        int id,
        decimal totalNet,
        decimal totalGross,
        OrderStatusDto status,
        int clientId,
        DateTime createdDate,
        DateTime modifiedDate,
        OrderAddressDto orderAddress,
        List<OrderItemDto> items)
    {
        Id = id;
        TotalNet = totalNet;
        TotalGross = totalGross;
        Status = status;
        ClientId = clientId;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        OrderAddress = orderAddress;
        Items = items;
    }

    public OrderDto() { }
    
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public decimal TotalNet { get; set; }

    [DataMember]
    public decimal TotalGross { get; set; }

    [DataMember]
    public OrderStatusDto Status { get; set; }

    [DataMember]
    public int ClientId { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }

    [DataMember]
    public DateTime ModifiedDate { get; set; }

    [DataMember]
    public OrderAddressDto OrderAddress { get; set; }

    [DataMember]
    public List<OrderItemDto> Items { get; set; }
}

[DataContract]
public class OrderItemDto
{
    public OrderItemDto(
        int id,
        decimal priceNet,
        decimal priceGross,
        int quantity,
        int productId,
        string? productName,
        string? productThumbnailUri)
    {
        Id = id;
        PriceNet = priceNet;
        PriceGross = priceGross;
        Quantity = quantity;
        ProductId = productId;
        ProductName = productName;
        ProductThumbnailUri = productThumbnailUri;
    }

    public OrderItemDto() { }

    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public decimal PriceNet { get; set; }

    [DataMember]
    public decimal PriceGross { get; set; }

    [DataMember]
    public int Quantity { get; set; }

    [DataMember]
    public int ProductId { get; set; }

    [DataMember]
    public string? ProductName { get; set; }

    [DataMember]
    public string? ProductThumbnailUri { get; set; }
}