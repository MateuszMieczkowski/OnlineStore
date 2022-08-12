using System.ComponentModel.DataAnnotations;

namespace SneakersBase.Shared.Models.Validators
{
    public class CreateProductDtoSizesValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var sizes = (List<CreateProductSizeDto>)value;
            var hs = new HashSet<int>();
            bool areSizesUnique = sizes.Where(s => s.SizeId.HasValue).All(x => hs.Add(x.SizeId.Value));

            if (!areSizesUnique)
            {
                ErrorMessage = "The product sizes have to be unique";
                return false;
            }
            return true;
        }
    }
}
