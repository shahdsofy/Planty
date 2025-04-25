using Blog_Platform.Models;
using Planty.DTOs;
using Planty.Models;
using Planty.Models.Enums;
using Planty.Repositories.Interfaces;
using Planty.Services.Interfaces;

namespace Planty.Services
{
	public class AdminDashboardService : IAdminDashboardService
	{
		private readonly IAdminDashboardRepository _repo;

		public AdminDashboardService(IAdminDashboardRepository repo)
		{
			_repo = repo;
		}

		public async Task<DashboardStatsDto> GetDashboardStatsAsync()
		{
			return new DashboardStatsDto
			{
				TotalUsers = await _repo.GetTotalUsersAsync(),
				TotalOrders = await _repo.GetTotalOrdersAsync(),
				TotalSales = await _repo.GetTotalSalesAsync()
			};
		}

		public Task<List<Order>> GetAllOrdersAsync() => _repo.GetAllOrdersAsync();

		public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
		{
			var order = await _repo.GetOrderByIdAsync(orderId);
			if (order == null) return false;
			order.Status = Enum.Parse<OrderStatus>(status);
			await _repo.SaveChangesAsync();
			return true;
		}

		public Task<List<Plant>> GetAllPlantsAsync() => _repo.GetAllPlantsAsync();

		public async Task<Plant?> CreatePlantAsync(Plant plant)
		{
			await _repo.AddPlantAsync(plant);
			await _repo.SaveChangesAsync();
			return plant;
		}

		public async Task<bool> UpdatePlantAsync(int id, Plant updatedPlant)
		{
			var existing = await _repo.GetPlantByIdAsync(id);
			if (existing == null) return false;
			existing.Name = updatedPlant.Name;
			existing.Type = updatedPlant.Type;
			existing.Price = updatedPlant.Price;
			existing.ImagePath = updatedPlant.ImagePath;
			existing.Details = updatedPlant.Details;
			await _repo.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeletePlantAsync(int id)
		{
			var plant = await _repo.GetPlantByIdAsync(id);
			if (plant == null) return false;
			await _repo.DeletePlantAsync(plant);
			await _repo.SaveChangesAsync();
			return true;
		}

		public Task<List<AppUser>> GetAllUsersAsync() => _repo.GetAllUsersAsync();

		public async Task<bool> DeleteUserAsync(string id)
		{
			var user = await _repo.GetUserByIdAsync(id);
			if (user == null) return false;
			await _repo.DeleteUserAsync(user);
			await _repo.SaveChangesAsync();
			return true;
		}
	}
}
