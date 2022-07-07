using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerBase.Shared.Dtos
{
    public class CreateProductSizeDto
    {
        [Required]
        public int SizeId { get; set; }
        [Range(1,10000, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
