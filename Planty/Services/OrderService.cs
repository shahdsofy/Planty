using Planty.Repositories.Interfaces;
using Planty.Services.Interfaces;
using Planty.Models;

namespace Planty.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _repo;

		public OrderService(IOrderRepository repo)
		{
			_repo = repo;
		}

		public async Task<List<Order>> GetUserOrdersAsync(string userId)
		{
			return await _repo.GetOrdersByUserIdAsync(userId);
		}

		public async Task<Order?> GetOrderStatusAsync(int orderId, string userId)
		{
			return await _repo.GetOrderByIdAsync(orderId, userId);
		}

		public async Task<bool> CancelOrderAsync(int orderId, string userId)
		{
			return await _repo.CancelOrderAsync(orderId, userId);
		}
	}
}
