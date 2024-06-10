using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Settings;
using MongoDB.Driver;

namespace BtoBInventoryAPI.Data
{
    public class AppDbContext : IDatabaseContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IMongoClient client, IMongoDBSettings settings)
        {
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Inventory> Inventories => _database.GetCollection<Inventory>("Inventories");
    }

    public interface IDatabaseContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Inventory> Inventories { get; }
    }
}
