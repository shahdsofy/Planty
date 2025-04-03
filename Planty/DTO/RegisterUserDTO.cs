using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$", ErrorMessage = "The email must be like this: name@domain.com")]
        public string Email { get; set; }
        [Required]
        [Length(8,20)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public class UpdateUserDTO : RegisterUserDTO
    {
        [Required]
        [Length(8,20)]
        public string OldPassword { get; set; }
    }

}
