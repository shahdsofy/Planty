
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.DTO;
using Planty.Models;
using Planty.Models.Enums;
using System.Security.Claims;

namespace Planty.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public OrderController(ApplicationDbContext context)
		{
			_context = context;
		}




		[HttpGet("GetAllOrders")]
		public async Task<IActionResult> GetUserOrders()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			int userIdInt = int.Parse(userId);

			var orders = await _context.Orders
				.Where(o => o.UserID == userIdInt)
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Plant)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();

			var result = orders.Select(o => new
			{
				o.OrderID,
				o.OrderDate,
				Status = o.Status.ToString(),
				PaymentMethod = o.PaymentMethod.ToString(),
				ShippingAddress = o.ShippingAddress,
				Notes = o.Notes,
				Items = o.OrderItems.Select(oi => new
				{
					oi.PlantID,
					PlantName = oi.Plant.Name,
					oi.Quantity,
					oi.Price
				})
			});

			return Ok(result);
		}


		[HttpGet("GetOrderStatus/{id}")]
		public async Task<IActionResult> GetOrderStatus(int id)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			int userIdInt = int.Parse(userId);

			var order = await _context.Orders
				.FirstOrDefaultAsync(o => o.OrderID == id && o.UserID == userIdInt);

			if (order == null)
				return NotFound(new { Message = "Order not found." });

			return Ok(new
			{
				OrderID = order.OrderID,
				Status = order.Status.ToString() // convert enum to string
			});
		}


		[HttpPost("Cancel/{id}")]
		public async Task<IActionResult> CancelOrder(int id)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			int userIdInt = int.Parse(userId);

			var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == id && o.UserID == userIdInt);

			if (order == null)
				return NotFound("Order not found");


			if (order.Status == OrderStatus.Shipped)
				return BadRequest("Cannot cancel shipped order");

			order.Status = OrderStatus.Canceled;
			await _context.SaveChangesAsync();

			return Ok("Order canceled successfully");
		}




	}

}
