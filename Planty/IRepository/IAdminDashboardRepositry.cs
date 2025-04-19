//namespace Planty.IRepository
//{
//	public interface IAdminDashboardRepositry
//	{
//	}
//}
using Blog_Platform.Models;
using Planty.Models;
using Planty.Models.Enums;

namespace Planty.Repositories.Interfaces
{
	public interface IAdminDashboardRepository
	{

		Task<int> GetTotalUsersAsync();
		Task<int> GetTotalOrdersAsync();
		Task<decimal> GetTotalSalesAsync();
		Task<List<Order>> GetAllOrdersAsync();
		Task<Order?> GetOrderByIdAsync(int id);
		//Control Plants
		Task<List<Plant>> GetAllPlantsAsync();
		Task<Plant?> GetPlantByIdAsync(int id);
		Task AddPlantAsync(Plant plant);
		Task UpdatePlantAsync(Plant plant);
		Task DeletePlantAsync(Plant plant);

		//Control Users
		Task<List<AppUser>> GetAllUsersAsync();
		Task<AppUser?> GetUserByIdAsync(string id);
		Task DeleteUserAsync(AppUser user);
		Task SaveChangesAsync();
	}
}
