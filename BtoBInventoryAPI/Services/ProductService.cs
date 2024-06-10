using BtoBInventoryAPI.Models;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _unitOfWork.Products.GetProductByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _unitOfWork.Products.AddProductAsync(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateProductAsync(Product product)   
        {
            await _unitOfWork.Products.UpdateProductAsync(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(string id)
        {
            await _unitOfWork.Products.DeleteProductAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
