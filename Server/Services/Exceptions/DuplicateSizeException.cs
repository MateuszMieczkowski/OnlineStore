namespace OnlineStore.Server.Services.Exceptions;

public class DuplicateSizeException : DuplicateException
{
    public DuplicateSizeException() : base("This size already exists")
    {
    }

    public DuplicateSizeException(string message) : base(message)
    {
    }
}