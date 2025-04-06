
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.DTO;
using Planty.IRepository;
using Planty.Models;

public class CartRepository : ICartRepository
{
	private readonly ApplicationDbContext _context;

	public CartRepository(ApplicationDbContext context)
	{
		_context = context;
	}
	public async Task<object> GetCartItemsByUserIdAsync(string userId)
	{
		int userIdInt = int.Parse(userId);

		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.ThenInclude(ci => ci.Plant)
			.FirstOrDefaultAsync(c => c.UserID == userIdInt);

		if (cart == null || !cart.CartItems.Any())
		{
			return new
			{
				Items = new List<object>(),
				Total = 0m
			};
		}

		var items = cart.CartItems.Select(ci => new
		{
			PlantId = ci.PlantID,
			PlantName = ci.Plant.Name,
			Price = ci.Plant.Price,
			Quantity = ci.Quantity,
			TotalPrice = ci.Quantity * ci.Plant.Price
		}).ToList();

		var total = items.Sum(i => i.TotalPrice);

		return new
		{
			Items = items,
			Total = total
		};
	}


	public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
	{
		int userIdInt = int.Parse(userId);

		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.FirstOrDefaultAsync(c => c.UserID == userIdInt);

		if (cart == null)
			return false;

		var item = cart.CartItems.FirstOrDefault(ci => ci.PlantID == productId);
		if (item == null)
			return false;

		_context.Remove(item);
		await _context.SaveChangesAsync();
		return true;
	}


	public async Task ClearCartAsync(string userId)
	{
		int userIdInt = int.Parse(userId);

		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.FirstOrDefaultAsync(c => c.UserID == userIdInt);

		if (cart != null)
		{
			_context.CartItems.RemoveRange(cart.CartItems);
			await _context.SaveChangesAsync();
		}
	}
	// Repositories/CartRepository.cs
	public async Task<bool> CheckoutAsync(string userId, CheckoutDTO checkoutData)
	{
		int userIdInt = int.Parse(userId);
		var cart = await _context.Carts
			.Include(c => c.CartItems)
			.ThenInclude(ci => ci.Plant)
			.FirstOrDefaultAsync(c => c.UserID == userIdInt);

		if (cart == null || cart.CartItems.Count == 0)
			return false;

		var newOrder = new Order
		{
			UserID = userIdInt,
			OrderDate = DateTime.Now,
			ShippingAddress = checkoutData?.ShippingAddress,
			Notes = checkoutData?.Notes,
			OrderItems = new List<OrderItem>()
		};

		foreach (var item in cart.CartItems)
		{
			newOrder.OrderItems.Add(new OrderItem
			{
				PlantID = item.PlantID,
				Quantity = item.Quantity,
				Price = item.Plant.Price // Assuming Plant has a Price property
			});
		}

		_context.Orders.Add(newOrder);

		// Clear cart
		_context.CartItems.RemoveRange(cart.CartItems);

		await _context.SaveChangesAsync();
		return true;
	}

}

