using System.ComponentModel.DataAnnotations;

namespace CDN.Models.DTO
{
    public class DirectoryCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
