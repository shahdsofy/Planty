using Planty.DTO;

namespace Planty.Services.Interfaces
{
	public interface ICartService
	{
		Task<bool> AddToCartAsync(string userId, CartItemDto dto);
		Task<object> GetCartItemsAsync(string userId);
		Task<bool> RemoveItemFromCartAsync(string userId, int productId);
		Task ClearCartAsync(string userId);
		Task<bool> CheckoutAsync(string userId, CheckoutDTO dto);
	}
}
