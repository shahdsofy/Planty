using Blog_Platform.Data;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Planty.Data;

namespace Blog_Platform.Repository
{
    public class TagRepo : Repo<Tag>, ITagRepo
    {
        public TagRepo(ApplicationDbContext context) : base(context) { }

        public bool CheckNameExistBefore(string name)
        {
            if(string.IsNullOrEmpty(name))
                return false;
            var Temp = name.ToLower();
            if (context.Tags.Any(x=>x.Name.ToLower() == Temp))
                return false;
            return true;
        }

        public int GetIdOfTag(string name)
        {
            var temp = name.ToLower();
            return context.Tags.First(x => x.Name.ToLower() == temp).Id;
        }
    }
}
