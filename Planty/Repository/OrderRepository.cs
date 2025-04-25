using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.Models;
using Planty.Models.Enums;
using Planty.Repositories.Interfaces;

namespace Planty.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
		{
			return await _context.Orders
				.Where(o => o.UserID == userId)
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Plant)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();
		}

		public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
		{
			return await _context.Orders
				.FirstOrDefaultAsync(o => o.OrderID == orderId && o.UserID == userId);
		}

		public async Task<bool> CancelOrderAsync(int orderId, string userId)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderId && o.UserID == userId);
			if (order == null || order.Status == OrderStatus.Shipped)
				return false;

			order.Status = OrderStatus.Canceled;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
