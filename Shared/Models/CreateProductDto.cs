using System.ComponentModel.DataAnnotations;
using SneakersBase.Shared.Models.Validators;

namespace SneakersBase.Shared.Models
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Model name is required.")]
        [MinLength(3, ErrorMessage = "Model name musi miec przynajmniej 3 znaki")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reference number is required.")]
        public string ReferenceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model image is required.")]
        public string ThumbnailPath { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "There must be at least one size.")]
        [CreateProductDtoSizesValidation]
        public List<CreateProductSizeDto> AvailableSizes { get; set; } = new ();
    }
}
