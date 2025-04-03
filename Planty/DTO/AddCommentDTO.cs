using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class AddCommentDTO
    {
        [Required]
        public int PostId { get; set; }//: Identifier for the blog post the comment belongs to.
        [Required]
        [MaxLength(300)]
        public string Content { get; set; }//: Main content of the comment.
    }
}
