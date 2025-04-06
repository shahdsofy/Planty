using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.Models;
using Planty.Models.Enums;

namespace Planty.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class AdminDashboardController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public AdminDashboardController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Get stats
		[HttpGet("Stats")]
		public async Task<IActionResult> GetStats()
		{
			var totalUsers = await _context.Users.CountAsync();
			var totalOrders = await _context.Orders.CountAsync();
			var totalSales = await _context.Orders
				.SelectMany(o => o.OrderItems)
				.SumAsync(i => i.Quantity * i.Plant.Price);

			return Ok(new
			{
				TotalUsers = totalUsers,
				TotalOrders = totalOrders,
				TotalSales = totalSales
			});
		}

		// Get all orders
		[HttpGet("Orders")]
		public async Task<IActionResult> GetAllOrders()
		{
			var orders = await _context.Orders
				//.Include(o => o.User)
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Plant)
				.ToListAsync();
			return Ok(orders);
		}

		// Update order status
		[HttpPut("OrderStatus/{id}")]
		public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order == null) return NotFound();
			order.Status = Enum.Parse<OrderStatus>(status);
			await _context.SaveChangesAsync();
			return Ok(order);
		}

		// Get all plants
		[HttpGet("Plants")]
		public async Task<IActionResult> GetAllPlants()
		{
			var plants = await _context.Plants.ToListAsync();
			return Ok(plants);
		}

		// Create new plant
		[HttpPost("Plant")]
		public async Task<IActionResult> CreatePlant([FromBody] Plant plant)
		{
			_context.Plants.Add(plant);
			await _context.SaveChangesAsync();
			return Ok(plant);
		}

		// Update existing plant
		[HttpPut("Plant/{id}")]
		public async Task<IActionResult> UpdatePlant(int id, [FromBody] Plant updatedPlant)
		{
			var plant = await _context.Plants.FindAsync(id);
			if (plant == null) return NotFound();

			plant.Name = updatedPlant.Name;
			plant.Type = updatedPlant.Type;
			plant.Price = updatedPlant.Price;
			plant.ImagePath = updatedPlant.ImagePath;
			plant.Details = updatedPlant.Details;

			await _context.SaveChangesAsync();
			return Ok(plant);
		}

		// Delete plant
		[HttpDelete("Plant/{id}")]
		public async Task<IActionResult> DeletePlant(int id)
		{
			var plant = await _context.Plants.FindAsync(id);
			if (plant == null) return NotFound();
			_context.Plants.Remove(plant);
			await _context.SaveChangesAsync();
			return Ok();
		}

		// Get all users
		[HttpGet("Users")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _context.Users.ToListAsync();
			return Ok(users);
		}

		// Delete user
		[HttpDelete("User/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
