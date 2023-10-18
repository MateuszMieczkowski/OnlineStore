namespace OnlineStore.Server.Services.Exceptions
{
    public class DuplicateSizeException : DuplicateException
    {
        public DuplicateSizeException() : base(message: "This size already exists")
        {

        }
        public DuplicateSizeException(string message) : base(message)
        {

        }
    }
}
