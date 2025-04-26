namespace Blog_Platform.DTO
{
    public class ShowBaseDataBlogPostDTO
    {
        public int Id { get; set; }  //: Unique identifier for the post.
     
        public string Content { get; set; } //: Main content of the blog post.
        public string AuthorName { get; set; }
        public string? User_Picture {  get; set; } 
        public string? Post_Picture {  get; set; } 
    }
}
