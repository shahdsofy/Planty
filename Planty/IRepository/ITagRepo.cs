using Blog_Platform.Models;

namespace Blog_Platform.IRepository
{
    public interface ITagRepo : IRepo<Tag>
    {
        bool CheckNameExistBefore(string name);
        int GetIdOfTag(string name);
    }
}
