using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Shared.Dtos
{
    public class CreateSizeDto
    {
        [Required(ErrorMessage = "Size name is required.")]
        [MinLength(2, ErrorMessage = "Size name must be at least 2 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
