namespace SneakersBase.Server.Services.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() : base(message: "This item already exists")
        {

        }
        public DuplicateException(string message) : base(message)
        {

        }
    }
}
