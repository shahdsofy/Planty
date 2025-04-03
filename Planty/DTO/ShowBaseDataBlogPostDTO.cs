namespace Blog_Platform.DTO
{
    public class ShowBaseDataBlogPostDTO
    {
        public int Id { get; set; }  //: Unique identifier for the post.
        public string Title { get; set; } //: Title of the blog post.
        public string Content { get; set; } //: Main content of the blog post.
        public string AuthorName { get; set; }
        public List<string> BlogPostHasTags { get; set; } = new List<string>();
    }
}
