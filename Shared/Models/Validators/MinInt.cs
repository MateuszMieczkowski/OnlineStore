using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Shared.Models.Validators;

public class MinInt : ValidationAttribute
{
    public int MinValue { get; set; }
    public string ValueName { get; set; }

    public override bool IsValid(object value)
    {
        if (value == null)
            return false;
        var integerValue = (int)value;
        if (integerValue < MinValue)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = $"{ValueName} must be grater than {MinValue}";
            return false;
        }

        return true;
    }
}