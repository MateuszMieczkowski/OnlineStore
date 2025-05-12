using OnlineStore.Shared.Infrastructure;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Orders;

public class CreateOrder : ICreateCommand
{
    public CreateOrder(int orderAddressId, List<CreateOrderItem> items)
    {
        OrderAddressId = orderAddressId;
        Items = items;
    }

    public CreateOrder() { }

    [XmlElement(Order = 0)]
    public int CreatedId { get; set; }

    [XmlArray(Order = 1)]
    public List<CreateOrderItem> Items { get; set; }

    [XmlElement(Order = 2)]
    public int OrderAddressId { get; set; }
};

public class CreateOrderItem
{
    public CreateOrderItem(int productId, int count)
    {
        ProductId = productId;
        Count = count;
    }

    public CreateOrderItem() { }

    public int Count { get; set; }

    public int ProductId { get; set; }
}