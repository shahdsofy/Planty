namespace Blog_Platform.DTO
{
    public class ShowAllDataOfBlogPostDTO: ShowBaseDataBlogPostDTO
    {
        public string AuthorId { get; set; } //: Identifier for the user who created the post.
        public DateTime CreatedDate { get; set; } //: Date and time the post was created.
        public DateTime UpdatedDate { get; set; }//: Date and time the post was last updated.
        public List<ShowShortDataCommentForPostDTO> Comments { get; set; } = new List<ShowShortDataCommentForPostDTO>(); //: List of Comment write on the post.
    }
}
