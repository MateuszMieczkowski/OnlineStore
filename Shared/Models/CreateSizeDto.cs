using System.ComponentModel.DataAnnotations;

namespace SneakersBase.Shared.Models
{
    public class CreateSizeDto
    {
        [Required(ErrorMessage = "Size name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}
