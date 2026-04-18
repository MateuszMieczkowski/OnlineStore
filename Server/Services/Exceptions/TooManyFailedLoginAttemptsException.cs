namespace OnlineStore.Server.Services.Exceptions
{
    public class TooManyFailedLoginAttemptsException : Exception
    {
        public int DelaySeconds { get; }

        public TooManyFailedLoginAttemptsException(int delaySeconds)
            : base($"Too many failed login attempts. Please wait {delaySeconds} seconds before trying again.")
        {
            DelaySeconds = delaySeconds;
        }
    }
}
