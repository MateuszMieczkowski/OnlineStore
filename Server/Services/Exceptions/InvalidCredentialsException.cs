namespace OnlineStore.Server.Services.Exceptions;

public class InvalidCredentialsException : BadRequestException
{
    public InvalidCredentialsException() : base("Wprowadzono niepoprawny adres e-mail lub hasło.")
    {
        
    }
}
