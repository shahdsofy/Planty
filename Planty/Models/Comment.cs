namespace Blog_Platform.Models
{
    public class Comment:IModelHelper
    {
        public int Id { get; set; } //: Unique identifier for the comment.
        public int PostId { get; set; }//: Identifier for the blog post the comment belongs to.
        public string AuthorId { get; set; }//: Identifier for the user who wrote the comment.

        public string AuthorName { get; set; }
        public string Content { get; set; }//: Main content of the comment.
        public DateTime CreatedDate { get; set; }//: Date and time the comment was created.
        public DateTime UpdatedDate { get; set; }//: Date and time the comment was created.
        public BlogPost BlogPost { get; set; } 
        public AppUser AppUser { get; set; }
    }
}
