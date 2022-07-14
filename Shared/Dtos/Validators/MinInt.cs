using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Shared.Dtos.Validators
{
    public class MinInt : ValidationAttribute
    {
        public int MinValue { get; set; }
        public string ValueName { get; set; }
        public override bool IsValid(object value)
        {
            if(value == null)
                return false;
            int integerValue = (int)value;
            if (integerValue < MinValue)
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                    ErrorMessage = $"{ValueName} must be grater than {MinValue}";
                return false;
            }
            return true;
        }
    }
}
