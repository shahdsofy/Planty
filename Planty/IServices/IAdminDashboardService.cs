using Blog_Platform.Models;
using Planty.DTOs;
using Planty.Models;

namespace Planty.Services.Interfaces
{
	public interface IAdminDashboardService
	{
		// Dashboard Stats
		Task<DashboardStatsDto> GetDashboardStatsAsync();

		// Orders
		Task<List<Order>> GetAllOrdersAsync();
		Task<bool> UpdateOrderStatusAsync(int orderId, string status);

		// Plants
		Task<List<Plant>> GetAllPlantsAsync();
		Task<Plant?> CreatePlantAsync(Plant plant);
		Task<bool> UpdatePlantAsync(int id, Plant updatedPlant);
		Task<bool> DeletePlantAsync(int id);

		// Users
		Task<List<AppUser>> GetAllUsersAsync();
		Task<bool> DeleteUserAsync(string id);
	}
}
