using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.DTO;
using Planty.IRepository;
using Planty.Models;

namespace Planty.Repository
{
	public class CartRepository : ICartRepository
	{
		private readonly ApplicationDbContext _context;

		public CartRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<object> GetCartItemsByUserIdAsync(string userId)
		{
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Plant)
				.FirstOrDefaultAsync(c => c.UserID == userId);

			if (cart == null || !cart.CartItems.Any())
				return new List<object>();

			return cart.CartItems.Select(item => new
			{
				item.PlantID,
				item.Plant.Name,
				item.Quantity,
				item.Plant.Price,
				Total = item.Quantity * item.Plant.Price
			}).ToList();
		}

		public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
		{
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userId);

			var item = cart?.CartItems.FirstOrDefault(ci => ci.PlantID == productId);
			if (item == null) return false;

			_context.CartItems.Remove(item);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task ClearCartAsync(string userId)
		{
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userId);

			if (cart != null)
			{
				_context.CartItems.RemoveRange(cart.CartItems);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<bool> CheckoutAsync(string userId, CheckoutDTO dto)
		{
			var cart = await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Plant)
				.FirstOrDefaultAsync(c => c.UserID == userId);

			if (cart == null || !cart.CartItems.Any())
				return false;

			var order = new Order
			{
				UserID = userId,
				OrderDate = DateTime.Now,
				ShippingAddress = dto?.ShippingAddress,
				Notes = dto?.Notes,
				OrderItems = cart.CartItems.Select(ci => new OrderItem
				{
					PlantID = ci.PlantID,
					Quantity = ci.Quantity,
					Price = ci.Plant.Price
				}).ToList(),
				Status = Models.Enums.OrderStatus.Pending,
				PaymentMethod = dto.PaymentMethod
			};

			await _context.Orders.AddAsync(order);
			_context.CartItems.RemoveRange(cart.CartItems);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<Cart?> GetCartByUserIdAsync(string userId)
		{
			return await _context.Carts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserID == userId);
		}

		public async Task CreateCartAsync(Cart cart)
		{
			await _context.Carts.AddAsync(cart);
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}


	}
}
