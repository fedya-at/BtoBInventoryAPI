using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Services
{
    public interface IInventoryServices
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(int id);
        Task AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(int id);
        Task<ScanResult> ScanNfcRfidAsync(ScanRequest scanRequest);
    }
}
