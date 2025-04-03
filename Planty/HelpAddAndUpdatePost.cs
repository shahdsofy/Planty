using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Serialization;

namespace Blog_Platform
{
    public static class HelpAddAndUpdatePost
    {
        public static void AddTagsForPost(List<string>Tags , int PostId , ITagRepo tagRepo ,IBlogPostHasTagRepo blogPostHasTagRepo )
        {
            blogPostHasTagRepo.DeleteByPostId( PostId );
            foreach (string item in Tags)
            {
                if (item.IsNullOrEmpty())
                    continue;
                if (tagRepo.CheckNameExistBefore(item))
                {
                    tagRepo.Add(new Tag() { Name = item });
                    tagRepo.Save();
                }
                blogPostHasTagRepo.Add(new BlogPostHasTag() { PostId = PostId, TagId = tagRepo.GetIdOfTag(item) });
                blogPostHasTagRepo.save();
            }
        }
    }
}
