using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Services
{
    public class InventoryService
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

        public async Task<Inventory> GetInventoryByIdAsync(int id)
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

        public async Task DeleteInventoryAsync(int id)
        {
            await _unitOfWork.Inventories.DeleteInventoryAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ScanResult> ScanNfcRfidAsync(ScanRequest scanRequest)
        {
            // Simulate the NFC/RFID scan process. You can replace this with actual NFC/RFID scanning logic.
            var inventoryItem = await _unitOfWork.Inventories.GetInventoryByTagIdAsync(scanRequest.TagId);
            if (inventoryItem == null)
            {
                return new ScanResult
                {
                    Success = false,
                    Message = "Item not found."
                };
            }

            return new ScanResult
            {
                Success = true,
                Message = "Item scanned successfully.",
                Inventory = inventoryItem
            };
        }
    }
}

