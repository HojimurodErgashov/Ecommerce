using System.ComponentModel.DataAnnotations;

namespace Ecommerce.V1.Users.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
