using BtoBInventoryAPI.Models;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Services
{
    public class InventoryService : IInventoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _unitOfWork.Inventories.GetAllInventoriesAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(string id)
        {
            return await _unitOfWork.Inventories.GetInventoryByIdAsync(id);
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            await _unitOfWork.Inventories.AddInventoryAsync(inventory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            await _unitOfWork.Inventories.UpdateInventoryAsync(inventory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteInventoryAsync(string id)
        {
            await _unitOfWork.Inventories.DeleteInventoryAsync(id);
            await _unitOfWork.CompleteAsync();
        }


     }
}

