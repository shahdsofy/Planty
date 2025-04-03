using Blog_Platform.Models;

namespace Blog_Platform.IRepository
{
    //public interface IRevokeRepo:IRepo<Revoke>
    //{
    //    bool CheckTokenIsRevoked(string token);
    //}
    public interface ITokenRepo
    {
        void Add(Token token);
        void Update(Token token);
        string? GetTokenByUserId(string UserId);
        void Delete(Token token);
        void Save();
        bool? CheckTokenIsRevoked(string UserId);
        bool IsExistBefore(string UserId);
        Token GetByUserId(string UserId);
    }
}
