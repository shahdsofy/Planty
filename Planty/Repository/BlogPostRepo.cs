using Blog_Platform.Data;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Planty.Data;

namespace Blog_Platform.Repository
{
    public class BlogPostRepo :Repo<BlogPost>,IBlogPostRepo
    {
        public BlogPostRepo(ApplicationDbContext context) : base(context) { }

        public List<BlogPost> GetAllPostsWithCommentsAndTags()
        {
            return context.Posts.Include(x=>x.Comments).Include(x=>x.AppUser).
                Include(x=>x.BlogPostHasTags).ThenInclude(x=>x.Tag).AsSingleQuery().ToList();
        }

        public int GetLastPostId()
        {
            return context.Posts.OrderBy(x=>x.CreatedDate).LastOrDefault()?.Id?? 0 ;
        }

        public BlogPost? GetPostByIdWithCommentsAndTags(int Id)
        {
            return context.Posts.Include(x => x.Comments).ThenInclude(x=>x.AppUser).Include(x => x.AppUser).
                Include(x => x.BlogPostHasTags).ThenInclude(x => x.Tag).AsSingleQuery().FirstOrDefault(x => x.Id == Id);
        }
    }
}
