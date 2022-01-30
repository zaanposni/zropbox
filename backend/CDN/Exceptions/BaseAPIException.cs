namespace CDN.Exceptions
{
    public class BaseAPIException : BaseException
    {
        public BaseAPIException(string? message) : base(message)
        {
        }

        public BaseAPIException() : base(null)
        {
        }
    }
}