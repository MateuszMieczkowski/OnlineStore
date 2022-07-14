﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SneakersBase.Shared.Dtos.Validators;

namespace SneakerBase.Shared.Dtos
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
