using System.Net.Mime;

namespace Zropbox.Models
{
    public class FileServe
    {
        public string Name { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public ContentDisposition ContentDisposition { get; set; }
    }
}
