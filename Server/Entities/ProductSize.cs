namespace OnlineStore.Server.Entities;

public class ProductSize
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public Guid? SizeId { get; set; }
    public virtual Product Product { get; set; }
    public virtual Size? Size { get; set; }
}