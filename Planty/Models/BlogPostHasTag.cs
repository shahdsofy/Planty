namespace Blog_Platform.Models
{
    public class BlogPostHasTag
    {
        
        public int TagId { get; set; } //: Unique identifier for the BlogPostHasTag and foreignKey for Tag.
        public int PostId { get; set; }//: Identifier for the blog post the Tag belongs to.
        public Tag Tag { get; set; }
        public BlogPost BlogPost { get; set; }

    }
}
