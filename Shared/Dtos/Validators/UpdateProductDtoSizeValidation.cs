using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Shared.Dtos.Validators
{
    public class UpdateProductDtoSizeValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var sizes = (List<UpdateProductSizeDto>)value;
            var hs = new HashSet<int>();
            bool areSizesUnique = sizes.All(x => hs.Add(x.SizeId));

            if (!areSizesUnique)
            {
                ErrorMessage = "The product sizes have to be unique";
                return false;
            }
            return true;
        }
    }
}
