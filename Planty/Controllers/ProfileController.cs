using Blog_Platform.DTO;
using Blog_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.DTO;
using System.Security.Claims;

namespace Planty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;

        public ProfileController(UserManager<AppUser>userManager) { 
            userManager=
this.userManager = userManager;
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await userManager.Users
                .Include(u=>u.orders)
                .Include(u => u.posts)
                .ThenInclude(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.ProfilePictureUrl,
                Orders = user.orders.Select(o => new { o.OrderDate, o.TotalPrice }),
                Posts = user.posts.Select(p => new
                {
                    p.Content,
                    p.PostPicture,
                    Comments = p.Comments.Select(c => new { c.Content, c.AuthorName, userManager.Users.FirstOrDefault(x => x.Id == c.AuthorId).ProfilePictureUrl })
                })
            });
        }
        [Authorize]
        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine("wwwroot/profile_picture", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            user.ProfilePictureUrl = $"/profile_picture/{fileName}";
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Profile picture updated", imageUrl = user.ProfilePictureUrl });
        }

  
    }
}
