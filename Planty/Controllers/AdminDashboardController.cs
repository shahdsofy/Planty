using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planty.Services.Interfaces;
using Planty.Models;
using Planty.Models.Enums;
using Planty.DTOs;

namespace Planty.Controllers
{
	[Route("api/admin/dashboard")]
	[ApiController]
	[Authorize(Roles = "ADMIN")]
	public class AdminDashboardController : ControllerBase
	{
		private readonly IAdminDashboardService _service;

		public AdminDashboardController(IAdminDashboardService service)
		{
			_service = service;
		}

		#region Dashboard (Stats)
		[HttpGet("Stats")]
		public async Task<IActionResult> GetStats()
		{
			var stats = await _service.GetDashboardStatsAsync();
			return Ok(stats);
		}
		#endregion

		#region Order Management
		[HttpGet("Orders")]
		public async Task<IActionResult> GetAllOrders()
		{
			var orders = await _service.GetAllOrdersAsync();
			return Ok(orders);
		}

		[HttpPut("OrderStatus/{id}")]
		public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
		{
			var result = await _service.UpdateOrderStatusAsync(id, status);
			return result ? Ok("Updated") : NotFound();
		}
		#endregion

		#region Plant Management
		[HttpGet("Plants")]
		public async Task<IActionResult> GetAllPlants()
		{
			var plants = await _service.GetAllPlantsAsync();
			return Ok(plants);
		}

		[HttpPost("Plant")]
		public async Task<IActionResult> CreatePlant([FromBody] Plant plant)
		{
			var created = await _service.CreatePlantAsync(plant);
			return Ok(created);
		}

		[HttpPut("Plant/{id}")]
		public async Task<IActionResult> UpdatePlant(int id, [FromBody] Plant updatedPlant)
		{
			var result = await _service.UpdatePlantAsync(id, updatedPlant);
			return result ? Ok(updatedPlant) : NotFound();
		}

		[HttpDelete("Plant/{id}")]
		public async Task<IActionResult> DeletePlant(int id)
		{
			var result = await _service.DeletePlantAsync(id);
			return result ? Ok() : NotFound();
		}
		#endregion

		#region Image Upload
		[HttpPost("UploadImage")]
		public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
		{
			if (imageFile == null || imageFile.Length == 0)
				return BadRequest("No file uploaded");

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "plants");
			Directory.CreateDirectory(uploadPath);
			var filePath = Path.Combine(uploadPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await imageFile.CopyToAsync(stream);
			}

			var imageUrl = $"{Request.Scheme}://{Request.Host}/images/plants/{fileName}";
			return Ok(new { ImageUrl = imageUrl });
		}
		#endregion

		#region User Management
		[HttpGet("Users")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _service.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpDelete("User/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var result = await _service.DeleteUserAsync(id);
			return result ? Ok() : NotFound();
		}
		#endregion
	}
}
