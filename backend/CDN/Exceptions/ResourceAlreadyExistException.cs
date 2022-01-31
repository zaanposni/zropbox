namespace CDN.Exceptions
{
    public class ResourceAlreadyExistException : BaseAPIException
    {
        public ResourceAlreadyExistException(string message) : base(message)
        {
        }
        public ResourceAlreadyExistException() : base("Resource already exists.")
        {
        }
    }
}
