using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BtoBInventoryAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductRepository(IDatabaseContext databaseContext)
        {
            _productsCollection = databaseContext.Products;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _productsCollection.Find(_ => true).ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            await _productsCollection.ReplaceOneAsync(filter, product);
        }

        public async Task DeleteProductAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            await _productsCollection.DeleteOneAsync(filter);
        }

       
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            var filter = Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Product>.Filter.Regex("Description", new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            );

            return await _productsCollection.Find(filter).ToListAsync();
        }
    }
}
