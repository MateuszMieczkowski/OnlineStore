using OnlineStore.Shared.Enums;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Orders;

[DataContract]
public class OrderListItemDto
{
    public OrderListItemDto(
        int id,
        decimal totalNet,
        decimal totalGross,
        int clientId,
        string clientEmail,
        DateTime createdDate)
    {
        Id = id;
        TotalNet = totalNet;
        TotalGross = totalGross;
        ClientId = clientId;
        ClientEmail = clientEmail;
        CreatedDate = createdDate;
    }

    public OrderListItemDto() { }
    
    [DataMember]
    public DateTime ModifiedDate { get; set; }

    [DataMember]
    public OrderStatusDto Status { get; set; }

    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public decimal TotalNet { get; set; }

    [DataMember]
    public decimal TotalGross { get; set; }

    [DataMember]
    public int ClientId { get; set; }

    [DataMember]
    public string ClientEmail { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }
};
