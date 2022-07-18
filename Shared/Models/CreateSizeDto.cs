using System.ComponentModel.DataAnnotations;

namespace SneakersBase.Shared.Models
{
    public class CreateSizeDto
    {
        [Required(ErrorMessage = "Size name is required.")]
        [MinLength(2, ErrorMessage = "Size name must be at least 2 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
