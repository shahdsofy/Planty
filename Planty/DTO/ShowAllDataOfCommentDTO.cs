using Blog_Platform.Models;

namespace Blog_Platform.DTO
{
    public class ShowAllDataOfCommentDTO
    {
        public int Id { get; set; } //: Unique identifier for the comment.
        public string AuthorId { get; set; }//: Identifier for the user who wrote the comment.
        public string Content { get; set; }//: Main content of the comment.
        public DateTime CreatedDate { get; set; }//: Date and time the comment was created.
        public DateTime UpdatedDate { get; set; }//: Date and time the comment was created.
        public string AuthorName { get; set; }
    }
}
