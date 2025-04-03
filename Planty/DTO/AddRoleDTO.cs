using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class AddRoleDTO
    {
        [Required]
        public string RoleName { get; set; }
    }
}
