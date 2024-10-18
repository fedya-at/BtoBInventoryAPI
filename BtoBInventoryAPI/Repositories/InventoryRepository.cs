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
        public async Task AddInventoryAsync(Inventory inventory, Product product)
        {
            using (var session = await _context.Inventories.Database.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    // Check if the product already exists
                    var existingProduct = await _context.Products.Find(p => p.Id == product.Id).FirstOrDefaultAsync();
                    if (existingProduct == null)
                    {
                        // Insert the new product if it doesn't exist
                        await _context.Products.InsertOneAsync(session, product);
                    }
                    else
                    {
                         // Update the existing product if it does exist
                        var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
                        await _context.Products.ReplaceOneAsync(session, filter, product);
                        
                    }

                    // Ensure the inventory's ProductId is set to the existing or new product's ID
                    inventory.ProductId = product.Id;

                    // Insert the new inventory
                    await _context.Inventories.InsertOneAsync(session, inventory);

                    await session.CommitTransactionAsync();
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
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

        
    }
}
