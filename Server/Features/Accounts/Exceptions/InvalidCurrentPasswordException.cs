using OnlineStore.Server.Services.Exceptions;

namespace OnlineStore.Server.Features.Accounts.Exceptions;

public class InvalidCurrentPasswordException : BadRequestException
{
    public InvalidCurrentPasswordException() : base("Podane obecne dane logowania są niepoprawne.")
    {
    }
}
