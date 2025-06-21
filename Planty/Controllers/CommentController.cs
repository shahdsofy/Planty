using Blog_Platform.DTO;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Blog_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo commentRepo;
        private readonly IBlogPostRepo postRepo;

        public CommentController(ICommentRepo commentRepo,IBlogPostRepo postRepo)
        {
            this.commentRepo = commentRepo;
            this.postRepo = postRepo;
        }
        [HttpPost]
        [Authorize]
        public ActionResult<GeneralResponse> AddComment(AddCommentDTO addComment)//: Allow users to add comments to blog posts.
        {
            if (ModelState.IsValid) 
            {
                
                Comment comment = new Comment() 
                {
                    AuthorId = User.Claims.First(x=>x.Type == ClaimTypes.NameIdentifier).Value,
                    Content = addComment.Content,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    PostId = addComment.PostId,
                    AuthorName= User.Claims.First(x => x.Type == ClaimTypes.Name).Value
                };
                commentRepo.Add(comment);
                commentRepo.Save();
                return new GeneralResponse()
                {
                    Success = false,
                    Content = "Add Comment Success"
                };
            }
            return new GeneralResponse() 
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage)
            };
        }
        [HttpGet]
        [Authorize/*(Roles = "Author,Admin")*/]
        public ActionResult<GeneralResponse> GetCommentsByPostId(int PostId)//: Retrieve all comments for a specific blog post.
        {
            if(postRepo.CheckIdExist(PostId))
            {
                List<Comment> comments = commentRepo.GetCommentsByPostIdWithUserData(PostId);
                List<ShowAllDataOfCommentDTO> commentDTOs = new List<ShowAllDataOfCommentDTO>();
                foreach (Comment comment in comments) 
                {
                    commentDTOs.Add(new ShowAllDataOfCommentDTO() 
                    {
                        AuthorId= comment.AuthorId,
                        AuthorName = comment.AppUser.UserName,
                        Content = comment.Content,
                        CreatedDate = comment.CreatedDate,
                        UpdatedDate = comment.UpdatedDate,
                        Id = comment.Id
                    });
                }
                return new GeneralResponse()
                {
                    Success = true,
                    Content = commentDTOs
                };
            }
            return new GeneralResponse() 
            {
                Success = false,
                Content = "Invalid Post Id"
            };
        }
        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> UpdateComment(UpdateCommentDTO updateComment)//: Allow users to update their own comments.
        {
            if (ModelState.IsValid) 
            {
                Comment comment = commentRepo.GetById(updateComment.Id)!;
                string UserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                if (UserId == comment.AuthorId)
                {
                    comment.Content = updateComment.Content;
                    comment.UpdatedDate = DateTime.Now;
                    commentRepo.Update(comment);
                    commentRepo.Save();
                    return new GeneralResponse()
                    {
                        Success = true,
                        Content = "Update Comment Success"
                    };
                }
                return new GeneralResponse() 
                {
                    Success= false,
                    Content = "You can't update Comment doesn't belong to you"
                };
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
            };
        }
        [HttpDelete]
        [Authorize]
        public ActionResult<GeneralResponse> DeleteComment(int Id)//: Allow users to delete their own comments.
        {
            if (commentRepo.CheckIdExist(Id)) 
            {
                string UserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                string AuthorId = commentRepo.GetAuthorIdOfComment(Id);
                if(UserId == AuthorId || User.IsInRole("ADMIN"))
                {
                    commentRepo.Delete(Id);
                    commentRepo.Save();
                    return new GeneralResponse()
                    {
                        Success = true,
                        Content = "Delete Comment success"
                    };
                }
                return new GeneralResponse()
                {
                    Success = false,
                    Content = "You Can't delete Comment doesn't belong to you"
                };
            }
            return new GeneralResponse() 
            {
                Success = false,
                Content = "Invalid Comment Id"
            };
        }
    
    }
}
