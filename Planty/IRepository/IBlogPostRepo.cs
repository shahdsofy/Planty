using Blog_Platform.Models;

namespace Blog_Platform.IRepository
{
    public interface IBlogPostRepo:IRepo<BlogPost>
    {
        List<BlogPost> GetAllPostsWithCommentsAndTags();
        BlogPost? GetPostByIdWithCommentsAndTags(int Id);
        int GetLastPostId();
    }
}
