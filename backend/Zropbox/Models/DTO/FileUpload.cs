using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models
{
    public class FileUpload
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public IFormFile File { set; get; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
