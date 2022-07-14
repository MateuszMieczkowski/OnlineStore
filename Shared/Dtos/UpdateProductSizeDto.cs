using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Shared.Dtos.Validators;

namespace SneakersBase.Shared.Dtos
{
    public class UpdateProductSizeDto
    {
    //    public int Id { get; set; } = default;
        [Required]
        [MinInt(MinValue = 1, ValueName = "Size", ErrorMessage = "Size must be chosen")]
        public int SizeId { get; set; }
     //   public string Size { get; set; }
        [MinInt(MinValue = 1, ValueName = "Quantity")]
        public int Quantity { get; set; }       
    }
}
