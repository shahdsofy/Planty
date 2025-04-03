using Blog_Platform.Models;

namespace Blog_Platform.IRepository
{
    public interface ICommentRepo : IRepo<Comment>
    {
        List<Comment> GetCommentsByPostIdWithUserData(int PostId);
        string GetAuthorIdOfComment(int Id);
        void DeleteByPostId(int PostId);
    }
}
