using Blog_Platform.Models;
using Planty.DTO;

namespace Blog_Platform.DTO
{
    public class ShowShortDataOfBlogPostDTO : ShowBaseDataBlogPostDTO
    {
     /*    public  List<KeyValuePair<string, string>> Comments { get; set; } = new List<KeyValuePair<string, string>>();*/ //: List of Comment write on the post.
        public List<showCommentDTO>Comments { get; set; } = new List<showCommentDTO>();
    }
}
