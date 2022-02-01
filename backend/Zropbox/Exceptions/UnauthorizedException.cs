namespace Zropbox.Exceptions
{
    public class UnauthorizedException : BaseAPIException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
        public UnauthorizedException() : base("You are not allowed to do that")
        {
        }
    }
}