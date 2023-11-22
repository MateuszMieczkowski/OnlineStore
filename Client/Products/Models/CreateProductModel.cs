using OnlineStore.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Products.Models;

public class CreateProductModel
{
    [Required(ErrorMessage = "Nazwa jest wymagane.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Numer referencyjny jest wymagane.")]
    public string ReferenceNumber { get; set; }

    public string? ShortDescription { get; set; }

    [Required(ErrorMessage = "Opis jest wymagane.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Ilość jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż zero.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Cena netto jest wymagane.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena netto musi być większa niż zero.")]
    public decimal PriceNet { get; set; }

    public bool IsHidden { get; set; }

    [Required(ErrorMessage = "Stawka podatku jest wymagane.")]
    public TaxRateDto TaxRate { get; set; }

    public ICollection<CreateProductFileModel> ProductFiles { get; set; } = new List<CreateProductFileModel>();
}