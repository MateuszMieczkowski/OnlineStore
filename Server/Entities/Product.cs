namespace OnlineStore.Server.Entities;

public class Product
{
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
    public TaxRate TaxRate { get; set; } = default!;
    public ProductCategory? ProductCategory { get; set; }
    public ICollection<ProductFile> ProductFiles { get; set; } = default!;
}