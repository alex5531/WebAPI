using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(45)]
        public string Email { get; set; }
        [Required(ErrorMessage = "You must have a password")]
        [StringLength(45, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
