using Blog_Platform.DTO;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Blog_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepo blogPostRepo;
        private readonly ITagRepo tagRepo;
        private readonly IBlogPostHasTagRepo blogPostHasTagRepo;
        private readonly ICommentRepo commentRepo;

        public BlogPostController(IBlogPostRepo blogPostRepo, ITagRepo tagRepo,IBlogPostHasTagRepo blogPostHasTagRepo,ICommentRepo commentRepo)
        {
            this.blogPostRepo = blogPostRepo;
            this.tagRepo = tagRepo;
            this.blogPostHasTagRepo = blogPostHasTagRepo;
            this.commentRepo = commentRepo;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Author")]
        public ActionResult<GeneralResponse> CreatePost(AddBlogPostDTO blogPostDTO)//: Allow authenticated users to create new blog posts.
        {
            if (ModelState.IsValid)
            { 
                BlogPost post = new BlogPost()
                {
                    Title = blogPostDTO.Title,
                    Content = blogPostDTO.Content,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    AuthorId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value
                };
                blogPostRepo.Add(post);
                blogPostRepo.Save();
                int postId = blogPostRepo.GetLastPostId();
                if (postId != 0)
                {
                    HelpAddAndUpdatePost.AddTagsForPost(blogPostDTO.Tags, postId, tagRepo, blogPostHasTagRepo);
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
                Content = ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage)
            };
        }
        [HttpGet("GetAll")]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllPosts()//: Retrieve a list of all blog posts.
        {
            List<BlogPost> posts = blogPostRepo.GetAllPostsWithCommentsAndTags();
            List<ShowShortDataOfBlogPostDTO> postDTOs = new List<ShowShortDataOfBlogPostDTO>();
            foreach (BlogPost post in posts) 
            {
                ShowShortDataOfBlogPostDTO showData = new ShowShortDataOfBlogPostDTO() 
                {
                    Title = post.Title,
                    Content = post.Content,
                    AuthorName = post.AppUser.UserName,
                    Id = post.Id
                };
                foreach (var item in post.Comments)
                    showData.Comments.Add(item.Content);
                foreach (var item in post.BlogPostHasTags)
                    showData.BlogPostHasTags.Add(item.Tag.Name);
                postDTOs.Add(showData);
            }
            return new GeneralResponse() 
            {
                Success = true,
                Content = postDTOs
            };
        }
        [HttpGet("{Id:int}")]
        [Authorize(Roles = "Admin,Author")]
        public ActionResult<GeneralResponse> GetPostById(int Id)//: Retrieve details of a specific blog post.
        { 
            BlogPost? post = blogPostRepo.GetPostByIdWithCommentsAndTags(Id);
            if (post is not null)
            {
                ShowAllDataOfBlogPostDTO showData = new ShowAllDataOfBlogPostDTO() 
                {
                    Title = post.Title,
                    Content = post.Content,
                    UpdatedDate = post.UpdatedDate,
                    CreatedDate = post.CreatedDate,
                    Id = post.Id,
                    AuthorName = post.AppUser.UserName,
                    AuthorId = post.AppUser.Id,
                };
                foreach (var item in post.Comments)
                {
                    ShowShortDataCommentForPostDTO commentForPostDTO = new ShowShortDataCommentForPostDTO() 
                    {
                        AuthorName = item.AppUser.UserName,
                        Id = item.Id,
                        Content = item.Content,
                        CreatedDate = item.CreatedDate
                    };
                    showData.Comments.Add(commentForPostDTO);
                }
                foreach (var item in post.BlogPostHasTags)
                    showData.BlogPostHasTags.Add(item.Tag.Name);
                return new GeneralResponse() 
                {
                    Success = true,
                    Content = showData
                };
            }
            return new GeneralResponse() 
            { 
                Success = false,
                Content = "Invalid Id for Post" 
            };
        }
        [HttpPut]
        [Authorize(Roles = "Admin,Author")]
        public ActionResult<GeneralResponse> UpdatePost(UpdatePostDTO updatePost)//: Allow users to update their own blog posts.
        {
            if (ModelState.IsValid)
            {
                BlogPost post = blogPostRepo.GetById(updatePost.Id)!;
                string UserId = User.Claims.First(x=>x.Type == ClaimTypes.NameIdentifier).Value;
                if(UserId == post.AuthorId)
                {
                    post.Title = updatePost.Title;
                    post.Content = updatePost.Content;
                    post.UpdatedDate = DateTime.Now;
                    blogPostRepo.Update(post);
                    blogPostRepo.Save();
                    HelpAddAndUpdatePost.AddTagsForPost(updatePost.Tags, updatePost.Id ,tagRepo, blogPostHasTagRepo);
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
                Content = ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage)
            };

        }
        [HttpDelete]
        [Authorize(Roles = "Admin,Author")]
        public ActionResult<GeneralResponse> DeletePost(int Id)//: Allow users to delete their own blog posts.
        {
            BlogPost? post = blogPostRepo.GetById(Id);
            if(post is not null)
            {
                var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                if (userId == post.AuthorId || User.IsInRole("Admin")) 
                {
                    commentRepo.DeleteByPostId(Id);
                    blogPostRepo.Delete(Id);
                    blogPostRepo.Save(); 
                    return new GeneralResponse() 
                    {
                        Success= true,
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
