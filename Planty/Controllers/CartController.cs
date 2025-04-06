// Controllers/CartController.cs
using Blog_Platform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.DTO;
using Planty.IRepository;
using Planty.Models;
using System.Security.Claims;

namespace Planty.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly ICartRepository cartRepository;

		public CartController(ApplicationDbContext context, ICartRepository cartRepository)
		{
			_context = context;
			this.cartRepository = cartRepository;
		}



		[HttpPost("AddToCart")]
		[Authorize]
		public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
		{

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			int userIdInt = int.Parse(userId);

			if (userId == null)
				return Unauthorized();

			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userIdInt);

			if (cart == null)
			{
				cart = new Cart
				{
					UserID = userIdInt,
					CartItems = new List<CartItem>()
				};
				_context.Carts.Add(cart);
			}

			var existingItem = cart.CartItems
				.FirstOrDefault(ci => ci.PlantID == dto.PlantID);

			if (existingItem != null)
			{
				existingItem.Quantity += dto.Quantity;
			}
			else
			{
				cart.CartItems.Add(new CartItem
				{
					PlantID = dto.PlantID,
					Quantity = dto.Quantity
				});
			}

			await _context.SaveChangesAsync();
			return Ok("Item added to cart successfully");
		}

		[HttpGet("GetCart")]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> GetCartItems()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
			var cartItems = await cartRepository.GetCartItemsByUserIdAsync(userId);

			return new GeneralResponse()
			{
				Success = true,
				Content = cartItems
			};
		}
		[HttpDelete("RemoveFromCart/{productId}")]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> RemoveFromCart(int productId)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

			bool isRemoved = await cartRepository.RemoveItemFromCartAsync(userId, productId);

			if (isRemoved)
			{
				return new GeneralResponse()
				{
					Success = true,
					Content = "Item removed successfully."
				};
			}

			return new GeneralResponse()
			{
				Success = false,
				Content = "Item not found or already removed."
			};
		}



		[HttpDelete("ClearCart")]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> ClearCart()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

			await cartRepository.ClearCartAsync(userId);

			return new GeneralResponse()
			{
				Success = true,
				Content = "Cart cleared successfully."
			};
		}

		[HttpPost("checkout")]
		[Authorize]
		public async Task<ActionResult<GeneralResponse>> Checkout([FromBody] CheckoutDTO dto)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

			bool isCheckedOut = await cartRepository.CheckoutAsync(userId, dto);

			if (isCheckedOut)
			{
				return new GeneralResponse
				{
					Success = true,
					Content = "Order created successfully."
				};
			}

			return new GeneralResponse
			{
				Success = false,
				Content = "Checkout failed. Cart might be empty."
			};
		}




	}
}
