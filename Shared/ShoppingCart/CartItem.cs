using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.ShoppingCart;

[DataContract]
[XmlRoot("GetShoppingCartResult")]
public class ShoppingCartDto
{
    [DataMember]
    public List<ShoppingCartItemDto> Items { get; set; }

    public ShoppingCartDto() { }

    public ShoppingCartDto(List<ShoppingCartItemDto> items)
    {
        Items = items;
    }
}

[DataContract]
public class ShoppingCartItemDto
{
    [DataMember]
    [XmlElement(Order = 0)]
    public int Count { get; set; }
    
    [DataMember]
    [XmlElement(Order = 1)]
    public string Name { get; set; }
    
    [DataMember]
    [XmlElement(Order = 2)]
    public int ProductId { get; set; }

    [DataMember]
    [XmlElement(Order = 3)]
    public string ThumbnailUri { get; set; }

    public ShoppingCartItemDto() { }

    public ShoppingCartItemDto(int productId, string name, string thumbnailUri, int count)
    {
        ProductId = productId;
        Name = name;
        ThumbnailUri = thumbnailUri;
        Count = count;
    }
}

