using Blog_Platform.Models;

namespace Blog_Platform.IRepository
{
    public interface IBlogPostRepo:IRepo<BlogPost>
    {
        List<BlogPost> GetAllPostsWithComments();
        BlogPost? GetPostByIdWithComments(int Id);
        int GetLastPostId();
    }
}
