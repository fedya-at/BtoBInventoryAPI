using BtoBInventoryAPI.Hubs;
using BtoBInventoryAPI.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Services
{
    public class InventoryService : IInventoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<InventoryHub> _hubContext;


        public InventoryService(IUnitOfWork unitOfWork, IHubContext<InventoryHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;

        }

        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _unitOfWork.Inventories.GetAllInventoriesAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(string id)
        {
            return await _unitOfWork.Inventories.GetInventoryByIdAsync(id);
        }

        public async Task AddInventoryAsync(Inventory inventory, Product product)
        {
            // First, add the product to the products collection
            await _unitOfWork.Products.AddProductAsync(product);

            // After product is added, retrieve the Id and assign it to inventory
            inventory.ProductId = product.Id;

            // Then, add the inventory with the productId set
            await _unitOfWork.Inventories.AddInventoryAsync(inventory);

            // Complete the unit of work
            await _unitOfWork.CompleteAsync();
            await _hubContext.Clients.All.SendAsync("inventoryAdded", inventory);

        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            await _unitOfWork.Inventories.UpdateInventoryAsync(inventory);
            await _unitOfWork.CompleteAsync();
            await _hubContext.Clients.All.SendAsync("inventoryUpdated", inventory);

        }

        public async Task DeleteInventoryAsync(string id)
        {
            await _unitOfWork.Inventories.DeleteInventoryAsync(id);
            await _unitOfWork.CompleteAsync();
            await _hubContext.Clients.All.SendAsync("inventoryDeleted", id);

        }


    }
}

