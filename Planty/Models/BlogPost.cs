namespace Blog_Platform.Models
{
    public class BlogPost:IModelHelper
    {
        public int Id { get; set; }  //: Unique identifier for the post.
        public string Content { get; set; } //: Main content of the blog post.
        public string AuthorId { get; set; } //: Identifier for the user who created the post.
        public DateTime CreatedDate { get; set; } //: Date and time the post was created.
        public DateTime UpdatedDate { get; set; }//: Date and time the post was last updated.
        public AppUser AppUser { get; set; }

        public string? PostPicture { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>(); //: List of Comment write on the post.

    }
}
