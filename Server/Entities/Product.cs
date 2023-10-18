namespace OnlineStore.Server.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ReferenceNumber { get; set; }
    public virtual List<ProductSize> AvailableSizes { get; set; }
}