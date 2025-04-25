
namespace Planty.Repositories.Interfaces
{
	public interface IOrderRepository
	{
		Task<List<Order>> GetOrdersByUserIdAsync(string userId);
		Task<Order?> GetOrderByIdAsync(int orderId, string userId);
		Task<bool> CancelOrderAsync(int orderId, string userId);
	}
}

