using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models.DTO
{
    public class FileUpload
    {
        [Required]
        public IFormFile File { set; get; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
