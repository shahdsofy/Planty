using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Planty.Data;
using Planty.Models;
using Planty.Repositories.Interfaces;

namespace Planty.Repositories
{
	public class AdminDashboardRepository : IAdminDashboardRepository
	{
		private readonly ApplicationDbContext _context;

		public AdminDashboardRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public Task<int> GetTotalUsersAsync() => _context.Users.CountAsync();
		public Task<int> GetTotalOrdersAsync() => _context.Orders.CountAsync();
		public Task<decimal> GetTotalSalesAsync() => _context.Orders
			.SelectMany(o => o.OrderItems)
			.SumAsync(i => i.Quantity * i.Plant.Price);

		public Task<List<Order>> GetAllOrdersAsync() => _context.Orders
			.Include(o => o.OrderItems)
			.ThenInclude(i => i.Plant)
			.ToListAsync();

		public Task<Order?> GetOrderByIdAsync(int id) => _context.Orders.FindAsync(id).AsTask();

		public Task<List<Plant>> GetAllPlantsAsync() => _context.Plants.ToListAsync();
		public Task<Plant?> GetPlantByIdAsync(int id) => _context.Plants.FindAsync(id).AsTask();
		public async Task AddPlantAsync(Plant plant) => await _context.Plants.AddAsync(plant);
		public Task UpdatePlantAsync(Plant plant)
		{
			_context.Plants.Update(plant);
			return Task.CompletedTask;
		}
		public Task DeletePlantAsync(Plant plant)
		{
			_context.Plants.Remove(plant);
			return Task.CompletedTask;
		}

		public Task<List<AppUser>> GetAllUsersAsync() => _context.Users.ToListAsync();
		public Task<AppUser?> GetUserByIdAsync(string id) => _context.Users.FindAsync(id).AsTask();
		public Task DeleteUserAsync(AppUser user)
		{
			_context.Users.Remove(user);
			return Task.CompletedTask;
		}

		public Task SaveChangesAsync() => _context.SaveChangesAsync();
	}
}
