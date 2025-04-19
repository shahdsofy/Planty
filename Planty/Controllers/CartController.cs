using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planty.DTO;
using Planty.Services.Interfaces;
using System.Security.Claims;

namespace Planty.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}

		[HttpPost("AddToCart")]
		public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null) return Unauthorized();

			bool result = await _cartService.AddToCartAsync(userId, dto);

			return result ? Ok("Item added successfully.") : BadRequest("Failed to add item.");
		}

		[HttpGet("GetCart")]
		public async Task<IActionResult> GetCartItems()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var cartItems = await _cartService.GetCartItemsAsync(userId);

			return Ok(new { Success = true, Content = cartItems });
		}

		[HttpDelete("RemoveFromCart/{productId}")]
		public async Task<IActionResult> RemoveFromCart(int productId)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var result = await _cartService.RemoveItemFromCartAsync(userId, productId);

			return Ok(new
			{
				Success = result,
				Content = result ? "Item removed successfully." : "Item not found."
			});
		}

		[HttpDelete("ClearCart")]
		public async Task<IActionResult> ClearCart()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			await _cartService.ClearCartAsync(userId);

			return Ok(new { Success = true, Content = "Cart cleared successfully." });
		}

		[HttpPost("Checkout")]
		public async Task<IActionResult> Checkout([FromBody] CheckoutDTO dto)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var result = await _cartService.CheckoutAsync(userId, dto);

			return Ok(new
			{
				Success = result,
				Content = result ? "Order created successfully." : "Checkout failed."
			});
		}
	}
}
