using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planty.Services.Interfaces;
using System.Security.Claims;

namespace Planty.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _service;

		public OrderController(IOrderService service)
		{
			_service = service;
		}

		[HttpGet("GetAllOrders")]
		public async Task<IActionResult> GetUserOrders()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var orders = await _service.GetUserOrdersAsync(userId);

			var result = orders.Select(o => new
			{
				o.OrderID,
				o.OrderDate,
				Status = o.Status.ToString(),
				PaymentMethod = o.PaymentMethod.ToString(),
				ShippingAddress = o.ShippingAddress,
				o.Notes,
				Items = o.OrderItems.Select(oi => new
				{
					oi.PlantID,
					PlantName = oi.Plant.Name,
					oi.Quantity,
				   total= oi.Price * oi.Quantity
				})
			});

			return Ok(result);
		}

		[HttpGet("GetOrderStatus/{id}")]
		public async Task<IActionResult> GetOrderStatus(int id)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var order = await _service.GetOrderStatusAsync(id, userId);

			if (order == null)
				return NotFound(new { Message = "Order not found." });

			return Ok(new
			{
				order.OrderID,
				Status = order.Status.ToString()
			});
		}

		[HttpPost("Cancel/{id}")]
		public async Task<IActionResult> CancelOrder(int id)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var canceled = await _service.CancelOrderAsync(id, userId);

			if (!canceled)
				return BadRequest("Order cannot be canceled or not found.");

			return Ok("Order canceled successfully");
		}
	}
}
