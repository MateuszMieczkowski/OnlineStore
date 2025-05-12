using OnlineStore.Shared.Infrastructure;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Products;

public class GetProduct : IQuery<ProductDto>
{
    public GetProduct()
    {
        
    }
    
    public GetProduct(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

[DataContract]
[XmlRoot("GetProductResult")]
public record ProductDto
{
    public ProductDto()
    {
        
    }
    
    public ProductDto(
        int Id,
        string Name,
        string ReferenceNumber,
        string? ShortDescription,
        string Description,
        string ThumbnailUri,
        int Quantity,
        decimal PriceNet,
        decimal PriceGross,
        bool IsHidden,
        bool IsDeleted,
        TaxRateDto TaxRate,
        IEnumerable<ProductFileDto> ProductFiles)
    {
        this.Id = Id;
        this.Name = Name;
        this.ReferenceNumber = ReferenceNumber;
        this.ShortDescription = ShortDescription;
        this.Description = Description;
        this.ThumbnailUri = ThumbnailUri;
        this.Quantity = Quantity;
        this.PriceNet = PriceNet;
        this.PriceGross = PriceGross;
        this.IsHidden = IsHidden;
        this.IsDeleted = IsDeleted;
        this.TaxRate = TaxRate;
        this.ProductFiles = ProductFiles.ToList();
    }

    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string ReferenceNumber { get; set; }

    [DataMember]
    public string? ShortDescription { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string ThumbnailUri { get; set; }

    [DataMember]
    public int Quantity { get; set; }

    [DataMember]
    public decimal PriceNet { get; set; }

    [DataMember]
    public decimal PriceGross { get; set; }

    [DataMember]
    public bool IsHidden { get; set; }

    [DataMember]
    public bool IsDeleted { get; set; }

    [DataMember]
    public TaxRateDto TaxRate { get; set; }

    [DataMember]
    public List<ProductFileDto> ProductFiles { get; set; }
}

[DataContract]
public class ProductFileDto
{
    public ProductFileDto()
    {
        
    }    
    
    public ProductFileDto(int id, string fileName, string blobUri, string? description, ProductFileTypeDto fileType)
    {
        Id = id;
        FileName = fileName;
        BlobUri = blobUri;
        Description = description;
        FileType = fileType;
    }

    
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string FileName { get; set; }

    [DataMember]
    public string BlobUri { get; set; }

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public ProductFileTypeDto FileType { get; set; }
}