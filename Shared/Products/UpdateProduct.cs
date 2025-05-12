using OnlineStore.Shared.Infrastructure;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Products;

public class UpdateProduct : ICommand
{
    [XmlElement(Order = 1)]
    public int Id { get; set; }

    [XmlElement(Order = 0)]
    public string Description { get; set; }

    [XmlElement(Order = 2)]
    public bool IsDeleted { get; set; }

    [XmlElement(Order = 3)]
    public bool IsHidden { get; set; }

    [XmlElement(Order = 4)]
    public string Name { get; set; }

    [XmlElement(Order = 5)]
    public decimal PriceNet { get; set; }

    [XmlArray(Order = 6)]
    public List<UpdateProductFileDto> ProductFiles { get; set; }

    [XmlElement(Order = 7)]
    public int Quantity { get; set; }

    [XmlElement(Order = 8)]
    public string ReferenceNumber { get; set; }

    [XmlElement(Order = 9)]
    public string? ShortDescription { get; set; }

    [XmlElement(Order = 10)]
    public int TaxRateId { get; set; }

    public UpdateProduct() { }

    public UpdateProduct(
        int id,
        string name,
        string referenceNumber,
        string? shortDescription,
        string description,
        int quantity,
        decimal priceNet,
        bool isHidden,
        bool isDeleted,
        int taxRateId,
        List<UpdateProductFileDto> productFiles)
    {
        Id = id;
        Name = name;
        ReferenceNumber = referenceNumber;
        ShortDescription = shortDescription;
        Description = description;
        Quantity = quantity;
        PriceNet = priceNet;
        IsHidden = isHidden;
        IsDeleted = isDeleted;
        TaxRateId = taxRateId;
        ProductFiles = productFiles;
    }
}
public record UpdateProductDto(
    string Name,
    string ReferenceNumber,
    string? ShortDescription,
    string Description,
    int Quantity,
    decimal PriceNet,
    bool IsHidden,
    bool IsDeleted,
    int TaxRateId,
    List<UpdateProductFileDto> ProductFiles)
{
    public UpdateProduct ToCommand(int productId) =>
        new(productId,
            Name,
            ReferenceNumber,
            ShortDescription,
            Description,
            Quantity,
            PriceNet,
            IsHidden,
            IsDeleted,
            TaxRateId,
            ProductFiles);
};

public class UpdateProductFileDto
{
    public UpdateProductFileDto(
        int? id,
        string fileName,
        string? fileBase64,
        ProductFileTypeDto productFileType,
        string? description)
    {
        Id = id;
        FileName = fileName;
        FileBase64 = fileBase64;
        ProductFileType = productFileType;
        Description = description;
    }

    public UpdateProductFileDto()
    {
        
    }

    [XmlElement(Order = 0)]
    public string? Description { get; set; }

    [XmlElement(Order = 1)]
    public string? FileBase64 { get; set; }

    [XmlElement(Order = 2)]
    public string FileName { get; set; }

    [XmlElement(Order = 3)]
    public int? Id { get; set; }
    
    [XmlElement(Order = 4)]
    public ProductFileTypeDto ProductFileType { get; set; }
}

    
    