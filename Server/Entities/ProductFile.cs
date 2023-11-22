using OnlineStore.Server.Enums;

namespace OnlineStore.Server.Entities;

public class ProductFile
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public Guid BlobId { get; set; }
    public string BlobUri { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public ProductFileType FileType { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}