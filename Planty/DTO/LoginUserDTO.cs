using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
