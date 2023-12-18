namespace OnlineStore.Client.Models.Orders;

public class CartModel
{
    public int AddressId { get; set; }
    
    public ICollection<CartItemModel> Items { get; set; } = new List<CartItemModel>();
}

public class CartItemModel
{
    public int ProductId { get; set; }
    
    public string Name { get; set; }
    
    public string ThumbnailUri { get; set; }

    public int Count { get; set; }
    
    public int ProductQuantity { get; set; }
}
