using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class GetUserIdAndRoleForRemoveDTO: BaseGetUserIdAndRoleDTO
    {
        [Required]
        [RegularExpression("(Admin|Author)")]
        public string RoleName { get; set; }
    }
}
