using System.ComponentModel.DataAnnotations;

namespace SneakersBase.Shared.Models.Validators
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
