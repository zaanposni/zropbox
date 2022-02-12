using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models
{
    public class UserRegister
    {
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
    }
}
