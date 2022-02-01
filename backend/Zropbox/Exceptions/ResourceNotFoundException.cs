namespace Zropbox.Exceptions
{
    public class ResourceNotFoundException : BaseAPIException
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
        public ResourceNotFoundException() : base("Resource not found.")
        {
        }
    }
}