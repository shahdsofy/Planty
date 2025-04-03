using Blog_Platform.Data;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Planty.Data;

namespace Blog_Platform.Repository
{
    public class CommentRepo : Repo<Comment>,ICommentRepo
    {
        public CommentRepo(ApplicationDbContext context) : base(context) { }

        public void DeleteByPostId(int PostId)
        {
            context.Comments.Where(x=>x.PostId == PostId).ExecuteDelete();
        }

        public string GetAuthorIdOfComment(int Id)
        {
            return context.Comments.First(x=>x.Id == Id).AuthorId;
        }

        public List<Comment> GetCommentsByPostIdWithUserData(int PostId)
        {
            return context.Comments.Where(x=>x.PostId == PostId).Include(x=>x.AppUser).ToList();
        }
    }
}
