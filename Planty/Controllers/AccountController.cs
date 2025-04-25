using Blog_Platform;
using Blog_Platform.DTO;
using Blog_Platform.IRepository;
using Blog_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Planty.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Planty.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AppUser> userManager;
		private readonly IConfiguration configuration;
		private readonly ITokenRepo tokenRepo;

		public AccountController(UserManager<AppUser> userManager, IConfiguration configuration, ITokenRepo tokenRepo)
		{
			this.userManager = userManager;
			this.configuration = configuration;
			this.tokenRepo = tokenRepo;
		}
		[HttpPost("Register")]
		public async Task<ActionResult<GeneralResponse>> Register(RegisterUserDTO registerUser) //: Allow new users to create an account.
		{
			if (ModelState.IsValid)
			{
				AppUser user = new AppUser()
				{
					UserName = registerUser.Username,
					Email = registerUser.Email,
				};
				IdentityResult identityResult = await userManager.CreateAsync(user, registerUser.Password);
				if (identityResult.Succeeded)
				{

                    string UserId = await userManager.GetUserIdAsync(user);
                    identityResult = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, registerUser.Username));
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, registerUser.Email));
                    await userManager.AddClaimAsync(user, new Claim("Password", registerUser.Password));
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, UserId));
                    identityResult = await userManager.AddToRoleAsync(user, "Author");
                    if (identityResult.Succeeded)
                    {
                        return new GeneralResponse()
                        {
                            Success = true,
                            Content = "Add user success"
                        };
                    }
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return new GeneralResponse()
            {
                Success = false,
                Content = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
            };
        }
        [HttpPost("Login")]
        public async Task<ActionResult<GeneralResponse>> Login(LoginUserDTO loginUser) //: Authenticate users and provide access tokens.
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await userManager.FindByNameAsync(loginUser.UserName);
                if (user is not null)
                {
                    bool check = await userManager.CheckPasswordAsync(user, loginUser.Password);
                    if (check)
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.Email, user.Email));
                        claims.Add(new Claim("Password", loginUser.Password));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var Roles = await userManager.GetRolesAsync(user);
                        foreach (var Role in Roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, Role));
                        }
                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
                        SigningCredentials signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken DescriptionToken = new JwtSecurityToken
                            (
                                issuer: configuration["JWT:issuer"],
                                audience: configuration["JWT:audience"],
                                claims: claims,
                                expires: DateTime.Now.AddDays(1),
                                signingCredentials: signing
                            );
                        string token = new JwtSecurityTokenHandler().WriteToken(DescriptionToken);
                        if (tokenRepo.IsExistBefore(user.Id))
                        {
                            var temp = tokenRepo.GetByUserId(user.Id);
                            temp.token = token;
                            temp.IsRevoked = false;
                            tokenRepo.Update(temp);
                        }
                        else
                        {
                            tokenRepo.Add(new Token()
                            {
                                UserId = user.Id,
                                token = token,
                                IsRevoked = false
                            });
                        }
                        tokenRepo.Save();
                        return new GeneralResponse()
                        {
                            Success = true,
                            Content = token
                        };
                    }
                    else
                    {
                        return new GeneralResponse()
                        {
                            Success = false,
                            Content = "Password or UserName invalid"
                        };
                    }
                }

			}
			return new GeneralResponse()
			{
				Success = false,
				Content = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
			};
		}
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> GetUserDetails()//: Retrieve details of a specific user.
		{
			GetDetailsOfUserDTO detailsOfUserDTO = new GetDetailsOfUserDTO()
			{
				Email = User.Claims.First(x => x.Type == ClaimTypes.Email)?.Value!,
				UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value!,
				UserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!,
				Password = User.Claims.FirstOrDefault(x => x.Type == "Password")?.Value!,
			};
			detailsOfUserDTO.Roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(detailsOfUserDTO.UserId)) as List<string>;
			return new GeneralResponse()
			{
				Success = true,
				Content = detailsOfUserDTO
			};
		}
		[HttpGet("GetAllUsers")]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> GetAllUsersDetails()//: Retrieve details of a specific user.
		{
			List<GetDetailsOfUserDTO> detailsOfUserDTOs = new List<GetDetailsOfUserDTO>();
			List<AppUser> users = userManager.Users.ToList();
			foreach (var user in users)
			{
				GetDetailsOfUserDTO detailsOfUserDTO = new GetDetailsOfUserDTO()
				{
					Email = user.Email,
					UserName = user.UserName,
					UserId = user.Id,
				};
				detailsOfUserDTOs.Add(detailsOfUserDTO);
			}
			return new GeneralResponse()
			{
				Success = true,
				Content = detailsOfUserDTOs
			};
		}
		[HttpPut]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> UpdateUser(UpdateUserDTO updateUser)//: Allow users to update their profile information.
		{
			if (ModelState.IsValid)
			{
				string UserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
				AppUser user = await userManager.FindByIdAsync(UserId);
				user.Id = UserId;
				user.Email = updateUser.Email;
				user.UserName = updateUser.Username;
				var IdentityResult = await userManager.UpdateAsync(user);
				if (IdentityResult.Succeeded)
				{
					await userManager.ReplaceClaimAsync(user, User.Claims.First(x => x.Type == ClaimTypes.Name), new Claim(ClaimTypes.Name, updateUser.Username));
					await userManager.ReplaceClaimAsync(user, User.Claims.First(x => x.Type == ClaimTypes.Email), new Claim(ClaimTypes.Email, updateUser.Email));
					IdentityResult = await userManager.ChangePasswordAsync(user, updateUser.OldPassword, updateUser.Password);
					if (IdentityResult.Succeeded)
					{
						return new GeneralResponse()
						{
							Success = true,
							Content = "Update User Success"
						};
					}
				}
				foreach (var item in IdentityResult.Errors)
					ModelState.AddModelError("", item.Description);
			}
			return new GeneralResponse()
			{
				Success = false,
				Content = ModelState.Values.SelectMany(c => c.Errors).Select(x => x.ErrorMessage)
			};
		}
		[HttpDelete]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> DeleteUser()//: Allow users to delete their account.
		{
			string UserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
			AppUser? user = await userManager.FindByIdAsync(UserId);
			if (user is not null)
			{
				IdentityResult identityResult = await userManager.DeleteAsync(user);
				if (identityResult.Succeeded)
				{
					var temp = tokenRepo.GetByUserId(UserId);
					temp.IsRevoked = true;
					tokenRepo.Update(temp);
					tokenRepo.Save();
					return new GeneralResponse()
					{
						Success = true,
						Content = "Delete User Success"
					};
				}
				foreach (var item in identityResult.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
				return new GeneralResponse()
				{
					Success = false,
					Content = ModelState.Values.SelectMany(c => c.Errors).Select(x => x.ErrorMessage)
				};
			}
			return new GeneralResponse()
			{
				Success = false,
				Content = "Not Found User Try Login and then Delete again"
			};
		}
		[HttpDelete("DeleteAnotherUser/{UserId}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<GeneralResponse>> DeleteAnotherUser(string UserId)//: Allow users to delete their account.
		{
			AppUser? user = await userManager.FindByIdAsync(UserId);
			if (user is not null)
			{
				IdentityResult identityResult = await userManager.DeleteAsync(user);
				if (identityResult.Succeeded)
				{
					var temp = tokenRepo.GetByUserId(UserId);
					temp.IsRevoked = true;
					tokenRepo.Update(temp);
					tokenRepo.Save();
					return new GeneralResponse()
					{
						Success = true,
						Content = "Delete User Success"
					};
				}
				foreach (var item in identityResult.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
				return new GeneralResponse()
				{
					Success = false,
					Content = ModelState.Values.SelectMany(c => c.Errors).Select(x => x.ErrorMessage)
				};
			}
			return new GeneralResponse()
			{
				Success = false,
				Content = "Invalid User Id"
			};
		}
		[HttpPost("RemoveUserFromRole")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<GeneralResponse>> RemoveUserFromRole(GetUserIdAndRoleForRemoveDTO UserIdAndRoleDTO)
		{
			AppUser? user = await userManager.FindByIdAsync(UserIdAndRoleDTO.UserId);
			if (user is not null)
			{
				var identityResult = await userManager.RemoveFromRoleAsync(user, UserIdAndRoleDTO.RoleName);
				if (identityResult.Succeeded)
				{
					bool checkExist = tokenRepo.IsExistBefore(UserIdAndRoleDTO.UserId);
					if (checkExist)
					{
						var temp = tokenRepo.GetByUserId(UserIdAndRoleDTO.UserId);
						temp.IsRevoked = true;
						tokenRepo.Update(temp);
						tokenRepo.Save();
					}
					return new GeneralResponse()
					{
						Success = true,
						Content = $"Remove User from Role {UserIdAndRoleDTO.RoleName} success"
					};
				}
				StringBuilder builder = new StringBuilder();
				foreach (var item in identityResult.Errors)
				{
					builder.Append($"{item.Description}\n");
				}
				return new GeneralResponse()
				{
					Success = true,
					Content = builder.ToString()
				};
			}
			return new GeneralResponse()
			{
				Success = false,
				Content = "Invalid User Id"
			};
		}
		[HttpPost("AddUserForRole")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<GeneralResponse>> AddUserToRole(GetUserIdAndRoleForAddDTO UserIdAndRoleDTO)
		{
			AppUser? user = await userManager.FindByIdAsync(UserIdAndRoleDTO.UserId);
			if (user is not null)
			{
				var identityResult = await userManager.AddToRoleAsync(user, UserIdAndRoleDTO.RoleName);
				if (identityResult.Succeeded)
				{

					bool CheckExist = tokenRepo.IsExistBefore(UserIdAndRoleDTO.UserId);
					if (CheckExist)
					{
						var temp = tokenRepo.GetByUserId(UserIdAndRoleDTO.UserId);
						temp.IsRevoked = true;
						tokenRepo.Update(temp);
						tokenRepo.Save();
					}
					return new GeneralResponse()
					{
						Success = true,
						Content = $"Add User to Role {UserIdAndRoleDTO.RoleName} success"
					};
				}
				StringBuilder builder = new StringBuilder();
				foreach (var item in identityResult.Errors)
				{
					builder.Append($"{item.Description}\n");
				}
				return new GeneralResponse()
				{
					Success = true,
					Content = builder.ToString()
				};
			}
			return new GeneralResponse()
			{
				Success = false,
				Content = "Invalid User Id"
			};
		}

	}
}
