using System.ComponentModel.DataAnnotations;
using OnlineStore.Shared.Models.Validators;

namespace OnlineStore.Shared.Models;

public class CreateProductDto
{
    [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Zdjęcie jest wymagane.")]
    public string ThumbnailPath { get; set; } = string.Empty;
}