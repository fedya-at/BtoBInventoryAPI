using BtoBInventoryAPI.Models;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Services
{
    public interface IInventoryServices
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(string id);
        Task AddInventoryAsync(Inventory inventory,Product product);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(string id);
    }
}
