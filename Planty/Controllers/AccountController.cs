using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Planty.Models;
using Planty.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Planty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration icong;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration Icong)
        {
            this.userManager = userManager;
            icong = Icong;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO DateFromRequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = DateFromRequest.UserName;
                user.Email = DateFromRequest.Email;
                


                IdentityResult identityResult = await userManager.CreateAsync(user, DateFromRequest.Password);
                if (identityResult.Succeeded)
                {
                    return Ok("Created");
                }
                else
                    foreach (var result in identityResult.Errors)
                    {
                        ModelState.AddModelError("Password", result.Description);
                    }

            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(UserFromRequest.UserName);

                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, UserFromRequest.Password);
                    if (found)
                    {
                        List<Claim> claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                        var userRoles = await userManager.GetRolesAsync(user);

                        foreach (var role in userRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var SignInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                icong["JWT:SecritKey"]));


                        var sigingcred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

                        //Design token

                        JwtSecurityToken token = new JwtSecurityToken(
                            audience: icong["JWT:AudienceIP"],
                            issuer: icong["JWT:IssuerIP"],
                            expires: DateTime.Now.AddHours(1),
                            claims: claims,
                            signingCredentials: sigingcred

                            );



                        //create token

                        return Ok(
                            new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = DateTime.Now.AddHours(1),
                            });


                    }
                }
                ModelState.AddModelError("Username", "Username OR Password  Invalid");


            }
            return BadRequest(ModelState);
        }
    }
}
