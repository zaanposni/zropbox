using System.ComponentModel.DataAnnotations;

namespace Zropbox.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
