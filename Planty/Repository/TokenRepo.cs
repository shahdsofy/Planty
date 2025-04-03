using Blog_Platform.Data;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Planty.Data;

namespace Blog_Platform.Repository
{
    //public class RevokeRepo : Repo<Revoke>, IRevokeRepo
    //{
    //    public RevokeRepo(AppDbContext context) : base(context) { }

    //    public bool CheckTokenIsRevoked(string token)
    //    {
    //        context.Revoke
    //    }
    //}
    public class TokenRepo : ITokenRepo
    {
        private readonly ApplicationDbContext context;

        public TokenRepo(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(Token token)
        {
            context.Add(token);
        }

        public bool? CheckTokenIsRevoked(string UserId)
        {
            return context.Tokens.FirstOrDefault(x => x.UserId == UserId)?.IsRevoked;
        }

        public void Delete(Token token)
        {
            context.Remove(token);
        }

        public Token GetByUserId(string UserId)
        {
            return context.Tokens.First(x => x.UserId == UserId);
        }

        public string? GetTokenByUserId(string UserId)
        {
            return context.Tokens.FirstOrDefault(x => x.UserId == UserId)?.token;
        }

        public bool IsExistBefore(string UserId)
        {
            return context.Tokens.Any(x => x.UserId == UserId);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Token token)
        {
            context.Update(token);
        }

        
    }
}
