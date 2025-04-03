using Blog_Platform.Models;

namespace Blog_Platform.IRepository
{
    public interface IBlogPostHasTagRepo
    {
        void Add(BlogPostHasTag blogPostHasTag);
        void save();
        void DeleteByPostId(int PostId);
    }
}
