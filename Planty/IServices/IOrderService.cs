using Planty.Models;

namespace Planty.Services.Interfaces
{
	public interface IOrderService
	{
		Task<List<Order>> GetUserOrdersAsync(string userId);
		Task<Order?> GetOrderStatusAsync(int orderId, string userId);
		Task<bool> CancelOrderAsync(int orderId, string userId);
	}
}
