using System.ComponentModel.DataAnnotations;
using OnlineStore.Shared.Models.Validators;

namespace OnlineStore.Shared.Models;

public class CreateProductSizeDto
{
    public Guid? SizeId { get; set; }

    public string? Size { get; set; }

    [MinInt(MinValue = 1, ValueName = "Quantity")]
    [Required]
    public int? Quantity { get; set; }
}