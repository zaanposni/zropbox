using System.ComponentModel.DataAnnotations;

namespace CDN.Models.DTO
{
    public class FileUpload
    {
        [Required]
        public IFormFile File { set; get; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
