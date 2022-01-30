namespace CDN.Exceptions
{
    public class InvalidConfigurationException : BaseException
    {
        public InvalidConfigurationException(string message) : base(message)
        {
        }
        public InvalidConfigurationException() : base("Invalid configuration.")
        {
        }
    }
}
