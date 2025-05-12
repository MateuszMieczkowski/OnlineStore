using OnlineStore.Shared.Infrastructure;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Products;

public class CreateProductsBatch : ICommand
{
    public CreateProductsBatch(List<CreateProductDto> products)
    {
        Products = products;
    }

    public CreateProductsBatch()
    {
        
    }
    
    public List<CreateProductDto> Products { get; set; }
}

public class CreateProductDto
{
    public CreateProductDto(
        string name,
        string referenceNumber,
        string? shortDescription,
        string description,
        int quantity,
        decimal priceNet,
        bool isHidden,
        int taxRateId,
        List<CreateProductFile> productFiles)
    {
        Name = name;
        ReferenceNumber = referenceNumber;
        ShortDescription = shortDescription;
        Description = description;
        Quantity = quantity;
        PriceNet = priceNet;
        IsHidden = isHidden;
        TaxRateId = taxRateId;
        ProductFiles = productFiles;
    }

    public CreateProductDto() { }

    [XmlElement(Order = 1)]
    public string Description { get; set; }

    [XmlElement(Order = 2)]
    public bool IsHidden { get; set; }

    [XmlElement(Order = 3)]
    public string Name { get; set; }

    [XmlElement(Order = 4)]
    public decimal PriceNet { get; set; }

    [XmlArray(Order = 5)]
    public List<CreateProductFile> ProductFiles { get; set; }

    [XmlElement(Order = 6)]
    public int Quantity { get; set; }

    [XmlElement(Order = 7)]
    public string ReferenceNumber { get; set; }

    [XmlElement(Order = 8)]
    public string? ShortDescription { get; set; }

    [XmlElement(Order = 9)]
    public int TaxRateId { get; set; }
}

public class CreateProductFile
{
    public CreateProductFile(string fileName, string fileBase64, ProductFileTypeDto productFileType, string? description)
    {
        FileName = fileName;
        FileBase64 = fileBase64;
        ProductFileType = productFileType;
        Description = description;
    }

    public CreateProductFile()
    {
    }

    [XmlElement(Order = 0)]
    public string? Description { get; set; }

    [XmlElement(Order = 1)]
    public string FileBase64 { get; set; }

    [XmlElement(Order = 2)]
    public string FileName { get; set; }

    [XmlElement(Order = 3)]
    public ProductFileTypeDto ProductFileType { get; set; }
}