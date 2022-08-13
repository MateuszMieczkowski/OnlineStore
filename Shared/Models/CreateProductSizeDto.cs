using System.ComponentModel.DataAnnotations;
using SneakersBase.Shared.Models.Validators;

namespace SneakersBase.Shared.Models
{
    public class CreateProductSizeDto
    {
        public Guid? SizeId { get; set; }

        public string? Size { get; set; }

        [MinInt(MinValue = 1, ValueName = "Quantity")]
        public int Quantity { get; set; }
    }
}
