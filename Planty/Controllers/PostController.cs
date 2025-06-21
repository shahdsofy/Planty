using Blog_Platform;
using Blog_Platform.DTO;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planty.DTO;
using System.Security.Claims;

namespace Planty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IBlogPostRepo blogPostRepo;
       
        private readonly ICommentRepo commentRepo;
        private readonly UserManager<AppUser> userManager;

        public PostController(IBlogPostRepo blogPostRepo, ICommentRepo commentRepo,UserManager<AppUser> userManager)
        {
            this.blogPostRepo = blogPostRepo;
            
            this.commentRepo = commentRepo;
            this.userManager = userManager;
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN,AUTHOR")]
        public async Task<ActionResult<GeneralResponse>> CreatePost([FromForm]  AddBlogPostDTO blogPostDTO, [FromForm]  IFormFile ? file)//: Allow authenticated users to create new blog posts.
        {
            if (ModelState.IsValid)
            {
                BlogPost post = new BlogPost()
                {
                    Content = blogPostDTO.Content,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    AuthorId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
                };

                if (file != null)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine("wwwroot/posts", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    post.PostPicture = $"/posts/{fileName}";

                }

                blogPostRepo.Add(post);
                blogPostRepo.Save();
                int postId = blogPostRepo.GetLastPostId();
                if (postId != 0)
                {
                    
                    return new GeneralResponse()
                    {
                        Success = true,
                        Content = "Add Post Success"
                    };
                }
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
            };
        }
        [HttpGet("GetAll")]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllPosts()//: Retrieve a list of all blog posts.
        {
            List<BlogPost> posts = blogPostRepo.GetAllPostsWithComments();
            List<ShowShortDataOfBlogPostDTO> postDTOs = new List<ShowShortDataOfBlogPostDTO>();
            foreach (BlogPost post in posts)
            {
                ShowShortDataOfBlogPostDTO showData = new ShowShortDataOfBlogPostDTO()
                {
                    Content = post.Content,
                    AuthorName = post.AppUser.UserName,
                    Id = post.Id,
                    User_Picture=post.AppUser.ProfilePictureUrl,
                    Post_Picture=post.PostPicture
                };
                
                foreach (var item in post.Comments)
                    showData.Comments.Add(new showCommentDTO
                    {
                        authorName = item.AuthorName,
                        content = item.Content,
                        Id= item.Id,
                        authorPicture = userManager.Users.FirstOrDefault(x => x.Id == item.AuthorId).ProfilePictureUrl
                    });

                postDTOs.Add(showData);
            }
            return new GeneralResponse()
            {
                Success = true,
                Content = postDTOs
            };
        }
        //[HttpGet("{Id:int}")]
        //[Authorize(Roles = "ADMIN,AUTHOR")]
        //public ActionResult<GeneralResponse> GetPostById(int Id)//: Retrieve details of a specific blog post.
        //{
        //    BlogPost? post = blogPostRepo.GetPostByIdWithComments(Id);
        //    if (post is not null)
        //    {
        //        ShowAllDataOfBlogPostDTO showData = new ShowAllDataOfBlogPostDTO()
        //        {
                  
        //            Content = post.Content,
        //            UpdatedDate = post.UpdatedDate,
        //            CreatedDate = post.CreatedDate,
        //            Id = post.Id,
        //            AuthorName = post.AppUser.UserName,
        //            AuthorId = post.AppUser.Id,
                   
        //            Post_Picture = post.PostPicture
        //        };
        //        foreach (var item in post.Comments)
        //        {
        //            ShowShortDataCommentForPostDTO commentForPostDTO = new ShowShortDataCommentForPostDTO()
        //            {
        //                AuthorName = item.AppUser.UserName,
        //                Id = item.Id,
        //                Content = item.Content,
        //                CreatedDate = item.CreatedDate
        //            };
        //            showData.Comments.Add(commentForPostDTO);
        //        }
               
        //        return new GeneralResponse()
        //        {
        //            Success = true,
        //            Content = showData
        //        };
        //    }
        //    return new GeneralResponse()
        //    {
        //        Success = false,
        //        Content = "Invalid Id for Post"
        //    };
        //}
        [HttpPut]
        [Authorize(Roles = "ADMIN,AUTHOR")]
        public ActionResult<GeneralResponse> UpdatePost(UpdatePostDTO updatePost)//: Allow users to update their own blog posts.
        {
            if (ModelState.IsValid)
            {
                BlogPost post = blogPostRepo.GetById(updatePost.Id)!;
                string UserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                if (UserId == post.AuthorId)
                {
                   
                    post.Content = updatePost.Content;
                    post.UpdatedDate = DateTime.Now;
                    blogPostRepo.Update(post);
                    blogPostRepo.Save();
                    
                    return new GeneralResponse()
                    {
                        Success = true,
                        Content = "Update Post Success"
                    };
                }
                return new GeneralResponse()
                {
                    Success = false,
                    Content = "You can't update Post Doesn't belong to you"
                };
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
            };

        }
        [HttpDelete]
        [Authorize(Roles = "ADMIN,AUTHOR")]
        public ActionResult<GeneralResponse> DeletePost(int Id)//: Allow users to delete their own blog posts.
        {
            BlogPost? post = blogPostRepo.GetById(Id);
            if (post is not null)
            {
                var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                if (userId == post.AuthorId || User.IsInRole("ADMIN"))
                {
                    commentRepo.DeleteByPostId(Id);
                    blogPostRepo.Delete(Id);
                    blogPostRepo.Save();
                    return new GeneralResponse()
                    {
                        Success = true,
                        Content = "Delete Post Success"
                    };
                }
                return new GeneralResponse()
                {
                    Success = false,
                    Content = "You can't delete Post doesn't belong to you"
                };
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = "Invalid Post Id"
            };
        }
    }
}
