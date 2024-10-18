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

        public IMongoCollection<Export> Exports => _database.GetCollection<Export>("Exports");
        public IMongoCollection<Import> Imports => _database.GetCollection<Import>("Imports");  

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    }
    public interface IDatabaseContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<Inventory> Inventories { get; }

        IMongoCollection<Export> Exports { get; }
        IMongoCollection<Import> Imports { get; }

        IMongoCollection<Category> Categories { get; }
    }


}
