using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class BaseGetUserIdAndRoleDTO
    {
        [Required]
        public string UserId { get; set; }
    }
}
