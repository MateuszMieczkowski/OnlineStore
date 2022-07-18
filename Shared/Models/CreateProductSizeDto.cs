using System.ComponentModel.DataAnnotations;
using SneakersBase.Shared.Models.Validators;

namespace SneakersBase.Shared.Models
{
    public class CreateProductSizeDto
    {
        [Required]
        [MinInt(MinValue = 1, ValueName = "Size", ErrorMessage = "Size must be chosen")]
        public int SizeId { get; set; }
        [MinInt(MinValue = 1, ValueName = "Quantity")]
        public int Quantity { get; set; }
    }
}
