namespace OnlineStore.Server.Entities;

public class OrderItemProduct
{
    public OrderItemProduct(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        ReferenceNumber = product.ReferenceNumber;
        ShortDescription = product.ShortDescription;
        Description = product.Description;
        PriceNet = product.PriceNet;
        PriceGross = product.PriceGross;
        IsHidden = product.IsHidden;
        IsDeleted = product.IsDeleted;
        Quantity = product.Quantity;
        ThumbnailBlobUri = product.ThumbnailBlobUri;
        TaxRateId = product.TaxRateId;
        ProductCategoryId = product.ProductCategoryId;
    }

    public OrderItemProduct()
    {
        
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal PriceNet { get; set; }
    public decimal PriceGross { get; set; }
    public bool IsHidden { get; set; }
    public bool IsDeleted { get; set; }
    public int Quantity { get; set; }
    public string ThumbnailBlobUri { get; set; } = string.Empty;
    public int TaxRateId { get; set; }
    public int? ProductCategoryId { get; set; }
}