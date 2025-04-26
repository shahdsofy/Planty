using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class AddBlogPostDTO
    {
        
        [Required]
        [MaxLength(300)]
        public string Content { get; set; } //: Main content of the blog post.
        public string? Picture {  get; set; } 
        
    }
}
