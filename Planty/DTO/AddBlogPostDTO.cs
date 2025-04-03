using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class AddBlogPostDTO
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } //: Title of the blog post.
        [Required]
        [MaxLength(300)]
        public string Content { get; set; } //: Main content of the blog post.
        [Required]
        [MaxLength(50)]
        public List<string> Tags { get; set; }
    }
}
