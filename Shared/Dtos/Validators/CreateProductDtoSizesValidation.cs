using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace SneakerBase.Shared.Dtos.Validators
{
    public class CreateProductDtoSizesValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var sizes = (List<CreateProductSizeDto>)value;
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
