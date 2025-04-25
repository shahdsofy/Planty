using Microsoft.EntityFrameworkCore;
using Planty.DTO;
using Planty.IRepository;
using Planty.Models;
using Planty.Services.Interfaces;

namespace Planty.Services
{
	public class CartService : ICartService
	{
		private readonly ICartRepository _cartRepo;

		public CartService(ICartRepository cartRepo)
		{
			_cartRepo = cartRepo;
		}

		public async Task<bool> AddToCartAsync(string userId, CartItemDto dto)
		{
			var cart = await _cartRepo.GetCartByUserIdAsync(userId);
			if (cart == null)
			{
				cart = new Cart
				{
					UserID = userId,
					CartItems = new List<CartItem>()
				};
				await _cartRepo.CreateCartAsync(cart);
			}

			var existingItem = cart.CartItems.FirstOrDefault(ci => ci.PlantID == dto.PlantID);

			if (existingItem != null)
				existingItem.Quantity += dto.Quantity;
			else
				cart.CartItems.Add(new CartItem
				{
					PlantID = dto.PlantID,
					Quantity = dto.Quantity
				});

			await _cartRepo.SaveAsync();
			return true;
		}

		public Task<object> GetCartItemsAsync(string userId)
			=> _cartRepo.GetCartItemsByUserIdAsync(userId);

		public Task<bool> RemoveItemFromCartAsync(string userId, int productId)
			=> _cartRepo.RemoveItemFromCartAsync(userId, productId);

		public Task ClearCartAsync(string userId)
			=> _cartRepo.ClearCartAsync(userId);

		public Task<bool> CheckoutAsync(string userId, CheckoutDTO dto)
			=> _cartRepo.CheckoutAsync(userId, dto);
	}
}
