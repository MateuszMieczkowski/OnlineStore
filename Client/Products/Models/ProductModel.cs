using OnlineStore.Shared.Products;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Products.Models;

public abstract class ProductModel<T> where T : ProductFileModel, new()
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Numer referencyjny jest wymagany.")]
    public string ReferenceNumber { get; set; }

    [Required(ErrorMessage = "Opis krótki jest wymagany.")]
    public string ShortDescription { get; set; }

    [Required(ErrorMessage = "Opis szczegółowy jest wymagany.")]
    [MaxLength(2000, ErrorMessage = "Opis może zawierać maksymalnie 2000 znaków.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Ilość jest wymagana.")]
    [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż zero.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Cena netto jest wymagana.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena netto musi być większa niż zero.")]
    public decimal PriceNet { get; set; }

    public bool IsHidden { get; set; }

    [Required(ErrorMessage = "Stawka podatku jest wymagana.")]
    public TaxRateDto? TaxRate { get; set; }

    [Required(ErrorMessage = "Miniaturka zdjęcia jest wymagana.")]
    public string ThumbnailImageSource { get; set; }

    public ICollection<T> ProductFiles { get; set; } = new List<T>();
}