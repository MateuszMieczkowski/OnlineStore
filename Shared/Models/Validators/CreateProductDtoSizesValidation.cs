// using System.ComponentModel.DataAnnotations;
//
// namespace OnlineStore.Shared.Models.Validators;
//
// public class CreateProductDtoSizesValidation : ValidationAttribute
// {
//     public override bool IsValid(object value)
//     {
//         var sizes = (List<CreateProductSizeDto>)value;
//         var hs = new HashSet<string>();
//         var areSizesUnique = sizes.Where(s => !string.IsNullOrEmpty(s.Size)).All(x => hs.Add(x.Size));
//
//         if (!areSizesUnique)
//         {
//             ErrorMessage = "The product sizes have to be unique";
//             return false;
//         }
//
//         return true;
//     }
// }