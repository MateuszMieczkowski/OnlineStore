namespace OnlineStore.Server.Entities;

public class ProductFile
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string BlobUri { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}