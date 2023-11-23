namespace OnlineStore.Server.Services.Exceptions;

public class RequestValidationException : Exception
{
    
    public RequestValidationException(IDictionary<string, string[]> errors) 
        : base(FormatErrorMessage(errors))
    {
        if (errors is null)
        {
            throw new ArgumentNullException(nameof(errors));
        }
    }
    
    private static string? FormatErrorMessage(IDictionary<string, string[]> errors)
    {
        if (!errors.Any()) return default;
        
        return errors.Aggregate("Validation failed: ",
            (current, error) => current + $"{error.Key}: {string.Join(", ", error.Value)}. ");
    }
}