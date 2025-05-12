using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Orders;

[DataContract]
[XmlRoot("GetAllOrdersResult")]
public class GetAllOrdersResponseDto
{
    [DataMember]
    public List<OrderListItemDto> Items { get; set; }

    [DataMember]
    public int PageNumber { get; set; }

    [DataMember]
    public int PageSize { get; set; }

    [DataMember]
    public int TotalPages { get; set; }

    [DataMember]
    public int TotalItemsCount { get; set; }

    public GetAllOrdersResponseDto(
        List<OrderListItemDto> items,
        int pageNumber,
        int pageSize,
        int totalPages,
        int totalItemsCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalItemsCount = totalItemsCount;
    }

    public GetAllOrdersResponseDto() { }
}