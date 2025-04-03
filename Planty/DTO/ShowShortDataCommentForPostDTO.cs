namespace Blog_Platform.DTO
{
    public class ShowShortDataCommentForPostDTO
    {
        public int Id { get; set; } //: Unique identifier for the comment.
        public string Content { get; set; }//: Main content of the comment.
        public DateTime CreatedDate { get; set; }//: Date and time the comment was created.
        public string AuthorName { get; set; }
    }
}
