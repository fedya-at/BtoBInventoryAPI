using BtoBInventoryAPI.Models;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<double> GetProductPriceByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId);

        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);


    }
}
