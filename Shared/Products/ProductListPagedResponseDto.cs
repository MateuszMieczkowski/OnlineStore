using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Products;

[DataContract]
[XmlRoot("GetProductListResult")]
public class ProductListPagedResponseDto
{
    [DataMember]
    public List<ProductListItemDto> Items { get; set; }

    [DataMember]
    public int PageNumber { get; set; }

    [DataMember]
    public int PageSize { get; set; }

    [DataMember]
    public int TotalPages { get; set; }

    [DataMember]
    public int TotalItemsCount { get; set; }

    public ProductListPagedResponseDto()
    {
        
    }    
    
    public ProductListPagedResponseDto(
        List<ProductListItemDto> items, 
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
}