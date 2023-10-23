namespace OnlineStore.Server.Services.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException() : base("This item already exists")
    {
    }

    public DuplicateException(string message) : base(message)
    {
    }
}