using Blog_Platform.Models;

namespace Blog_Platform.DTO
{
    public class ShowShortDataOfBlogPostDTO : ShowBaseDataBlogPostDTO
    {
        public List<string> Comments { get; set; } = new List<string>(); //: List of Comment write on the post.
    }
}
