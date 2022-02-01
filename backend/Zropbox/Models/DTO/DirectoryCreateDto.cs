using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models.DTO
{
    public class DirectoryCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
