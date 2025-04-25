using Planty.DTO;
using Planty.Models;

namespace Planty.IRepository
{
	public interface ICartRepository
	{
		Task<object> GetCartItemsByUserIdAsync(string userId);
		Task<bool> RemoveItemFromCartAsync(string userId, int productId);
		Task ClearCartAsync(string userId);
		Task<bool> CheckoutAsync(string userId, CheckoutDTO dto);
		Task<Cart?> GetCartByUserIdAsync(string userId);
		Task CreateCartAsync(Cart cart);
		Task SaveAsync();
	}
}
