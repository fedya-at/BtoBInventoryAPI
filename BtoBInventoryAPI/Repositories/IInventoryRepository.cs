using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int id);
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(int id);
        Task<Inventory> GetInventoryByTagIdAsync(string tagId);
    }
}
