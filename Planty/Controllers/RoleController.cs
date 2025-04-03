using Blog_Platform.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "Developer")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpPost]
        public async Task<GeneralResponse> Add(AddRoleDTO addRole)
        {
            if (ModelState.IsValid) 
            {
                IdentityRole role = new IdentityRole() 
                {
                    Name = addRole.RoleName 
                };
                IdentityResult identityResult= await roleManager.CreateAsync(role);
                if(identityResult.Succeeded)
                {
                    return new GeneralResponse() 
                    {
                        Success = true,
                        Content = "Add Role Success"
                    };
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return new GeneralResponse() 
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x=>x.Errors).Select(x => x.ErrorMessage)
            };
        }
    
    }
}
