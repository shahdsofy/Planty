
using Blog_Platform.IRepository;
using Blog_Platform.Repository;
using System.Security.Claims;

namespace Blog_Platform
{
    public class RevokeMiddleWare : IMiddleware
    {
        private readonly ITokenRepo tokenRepo;

        public RevokeMiddleWare(ITokenRepo tokenRepo)
        {
            this.tokenRepo = tokenRepo;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? UserId = context.User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (UserId is not null)
            {
                bool? check = tokenRepo.CheckTokenIsRevoked(UserId);
                if(check is not null && (bool) check)
                {
                    await context.Response.WriteAsync("Invalid Token Login again");
                    return;
                }
            }
            await next(context);
        }
    }
}
