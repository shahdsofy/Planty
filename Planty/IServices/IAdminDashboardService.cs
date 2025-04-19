using Blog_Platform.Models;
using Planty.DTOs;
using Planty.Models;

namespace Planty.Services.Interfaces
{
	public interface IAdminDashboardService
	{
		Task<DashboardStatsDto> GetDashboardStatsAsync();
		Task<List<Order>> GetAllOrdersAsync();
		Task<bool> UpdateOrderStatusAsync(int orderId, string status);

		Task<List<Plant>> GetAllPlantsAsync();
		Task<Plant?> CreatePlantAsync(Plant plant);
		Task<bool> UpdatePlantAsync(int id, Plant updatedPlant);
		Task<bool> DeletePlantAsync(int id);

		Task<List<AppUser>> GetAllUsersAsync();
		Task<bool> DeleteUserAsync(string id);
	}
}
