using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Authentication
{
    public class Register
    {
        [Required(ErrorMessage ="User Name is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage ="Role is required")]
        public string? Role { get; set; }

    }
}
