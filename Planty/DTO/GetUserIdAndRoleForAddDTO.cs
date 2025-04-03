using Blog_Platform.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class GetUserIdAndRoleForAddDTO: BaseGetUserIdAndRoleDTO
    {
        [Required]
        [RegularExpression("(Admin|Author|Reader)")]
        public string RoleName { get; set; }
    }
}
