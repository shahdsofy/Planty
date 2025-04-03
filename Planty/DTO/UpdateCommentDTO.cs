using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class UpdateCommentDTO
    {
        [Required]
        [CheckOnIdValid<Comment>]
        public int Id { get; set; } //: Unique identifier for the comment.
        [Required]
        [MaxLength(300)]
        public string Content { get; set; }//: Main content of the comment.
    }
}
