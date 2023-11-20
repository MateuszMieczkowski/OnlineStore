using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Accounts.Exceptions;

public class InvalidCurrentPasswordException : BadRequestException
{
    public InvalidCurrentPasswordException() : base("Podane obecne hasło jest niepoprawne.")
    {
    }
}
