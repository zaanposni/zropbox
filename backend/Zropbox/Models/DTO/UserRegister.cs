using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models
{
    public class UserRegister
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
    }
}
