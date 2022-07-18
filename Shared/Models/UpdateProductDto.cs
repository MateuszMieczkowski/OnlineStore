using System.ComponentModel.DataAnnotations;
using SneakersBase.Shared.Models.Validators;

namespace SneakersBase.Shared.Models
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Model name is required.")]
        [MinLength(3, ErrorMessage = "Model name must be at least 3 characters long")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reference number is required.")]
        public string ReferenceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model image is required.")]
        public string ThumbnailPath { get; set; } = string.Empty;
        
        public int Quantity { get; set; }

        [MinLength(1, ErrorMessage = "There must be at least one size.")]
        [UpdateProductDtoSizeValidation]
        public List<UpdateProductSizeDto> AvailableSizes { get; set; } = new();
    }
}
