using Microsoft.AspNetCore.Components.Forms;
using OnlineStore.Shared.Enums;
using OnlineStore.Shared.Products;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Products.Models;

public class CreateProductModel
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Numer referencyjny jest wymagany.")]
    public string ReferenceNumber { get; set; }

    public string? ShortDescription { get; set; }

    [Required(ErrorMessage = "Opis jest wymagany.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Ilość jest wymagana.")]
    [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż zero.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Cena netto jest wymagana.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena netto musi być większa niż zero.")]
    public decimal PriceNet { get; set; }

    public bool IsHidden { get; set; }

    [Required(ErrorMessage = "Stawka podatku jest wymagana.")]
    public TaxRateDto TaxRate { get; set; }


    [Required(ErrorMessage = "Miniaturka produktu jest wymagana.")]
    public IBrowserFile ThumbnailFile { get; set; }
    

    public string ThumbnailBase64 { get; set; }
    
    public ICollection<CreateProductFileModel> ProductFiles { get; set; } = new List<CreateProductFileModel>();
}
