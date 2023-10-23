using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Shared.Models;

public class UpdateSizeDto
{
    [Required(ErrorMessage = "Size name is required.")]
    [MinLength(2, ErrorMessage = "Size name must be at least 2 characters")]
    public string Name { get; set; }
}