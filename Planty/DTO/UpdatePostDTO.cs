using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class UpdatePostDTO:AddBlogPostDTO
    {
        [Required]
        [CheckOnIdValid<BlogPost>]
        public int Id { get; set; }
    }
}
