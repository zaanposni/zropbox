using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models
{
    public class FileEditDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        [Required]
        public int ParentId { get; set; }
    }
}
