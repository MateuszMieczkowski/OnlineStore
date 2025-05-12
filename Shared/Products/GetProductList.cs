using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Infrastructure;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Products;

public class GetProductList : IPagedQuery<ProductListItemDto>
{
    public GetProductList()
    {
        
    }
    
    public GetProductList(
        int pageNumber,
        int pageSize,
        bool deletedOnly = false,
        bool hiddenOnly = false,
        string? searchPhrase = null,
        string? name = null,
        string? referenceNumber = null,
        string? shortDescription = null,
        bool filterGrossPrice = true,
        decimal? priceFrom = null,
        decimal? priceTo = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        DeletedOnly = deletedOnly;
        HiddenOnly = hiddenOnly;
        SearchPhrase = searchPhrase;
        Name = name;
        ReferenceNumber = referenceNumber;
        ShortDescription = shortDescription;
        FilterGrossPrice = filterGrossPrice;
        PriceFrom = priceFrom;
        PriceTo = priceTo;
    }

    [DataMember(Order = 0)]
    public bool DeletedOnly { get; set; }

    [DataMember(Order = 1)]
    public bool FilterGrossPrice { get; set; }

    [DataMember(Order = 2)]
    public bool HiddenOnly { get; set; }

    [DataMember(Order = 3)]
    public string? Name { get; set; }

    [DataMember(Order = 4)]
    public int PageNumber { get; set; }

    [DataMember(Order = 5)]
    public int PageSize { get; set; }

    [DataMember(Order = 6)]
    public decimal? PriceFrom { get; set; }

    [DataMember(Order = 7)]
    public decimal? PriceTo { get; set; }

    [DataMember(Order = 8)]
    public string? ReferenceNumber { get; set; }

    [DataMember(Order = 9)]
    public string? SearchPhrase { get; set; }

    [DataMember(Order = 10)]
    public string? ShortDescription { get; set; }
}

[DataContract]
public class ProductListItemDto
{
    public ProductListItemDto()
    {
        
    }
    
    public ProductListItemDto(
        int id,
        string name,
        string referenceNumber,
        string? shortDescription,
        int quantity,
        decimal priceNet,
        decimal priceGross,
        string thumbnailUri)
    {
        Id = id;
        Name = name;
        ReferenceNumber = referenceNumber;
        ShortDescription = shortDescription;
        Quantity = quantity;
        PriceNet = priceNet;
        PriceGross = priceGross;
        ThumbnailUri = thumbnailUri;
    }

    [DataMember]
    public ProductStatusDto Status { get; set; }

    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string ReferenceNumber { get; set; }

    [DataMember]
    public string? ShortDescription { get; set; }

    [DataMember]
    public int Quantity { get; set; }

    [DataMember]
    public decimal PriceNet { get; set; }

    [DataMember]
    public decimal PriceGross { get; set; }

    [DataMember]
    public string ThumbnailUri { get; set; }
};