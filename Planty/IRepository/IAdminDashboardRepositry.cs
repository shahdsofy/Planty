using Blog_Platform.Models;
using Planty.Models;

namespace Planty.Repositories.Interfaces
{
	public interface IAdminDashboardRepository
	{
		// Dashboard Stats
		Task<int> GetTotalUsersAsync();
		Task<int> GetTotalOrdersAsync();
		Task<decimal> GetTotalSalesAsync();

		// Orders
		Task<List<Order>> GetAllOrdersAsync();
		Task<Order?> GetOrderByIdAsync(int id);

		// Plants
		Task<List<Plant>> GetAllPlantsAsync();
		Task<Plant?> GetPlantByIdAsync(int id);
		Task AddPlantAsync(Plant plant);
		Task UpdatePlantAsync(Plant plant);
		Task DeletePlantAsync(Plant plant);

		// Users
		Task<List<AppUser>> GetAllUsersAsync();
		Task<AppUser?> GetUserByIdAsync(string id);
		Task DeleteUserAsync(AppUser user);

		// Save
		Task SaveChangesAsync();
	}
}
