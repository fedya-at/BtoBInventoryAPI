using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BtoBInventoryAPI.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDatabaseContext _context;

        public InventoryRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            var cursor = await _context.Inventories.FindAsync(Builders<Inventory>.Filter.Empty);
            return await cursor.ToListAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(String id)
        {
            return await _context.Inventories.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            await _context.Inventories.InsertOneAsync(inventory);
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.Id, inventory.Id);
            await _context.Inventories.ReplaceOneAsync(filter, inventory);
        }

        public async Task DeleteInventoryAsync(String id)
        {
            var filter = Builders<Inventory>.Filter.Eq(i => i.Id, id);
            await _context.Inventories.DeleteOneAsync(filter);
        }

        public async Task<Inventory> GetInventoryByTagIdAsync(String tagId)
        {
            return await _context.Inventories.Find(i => i.TagId == tagId).FirstOrDefaultAsync();
        }
    }
}
