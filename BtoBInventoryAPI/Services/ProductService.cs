using BtoBInventoryAPI.Hubs;
using BtoBInventoryAPI.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ProductHub> _productHubContext;

        public ProductService(IUnitOfWork unitOfWork, IHubContext<ProductHub> productHubContext)
        {
            _unitOfWork = unitOfWork;
            _productHubContext = productHubContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _unitOfWork.Products.GetProductByIdAsync(id);
        }

        public async Task<double> GetProductPriceByIdAsync(string id)
        {
            return await _unitOfWork.Products.GetProductPriceByIdAsync(id);
        }
        public async Task AddProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _unitOfWork.Products.AddProductAsync(product);
            await _unitOfWork.CompleteAsync();

            // Notify clients about the new product
            await NotifyProductAddedAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _unitOfWork.Products.UpdateProductAsync(product);
            await _unitOfWork.CompleteAsync();

            // Notify clients about the updated product
            await NotifyProductUpdatedAsync(product);
        }

        public async Task DeleteProductAsync(string id)
        {
            var deletedProduct = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (deletedProduct != null)
            {
                await _unitOfWork.Products.DeleteProductAsync(id);
                await _unitOfWork.CompleteAsync();

                // Notify clients about the deleted product
                await NotifyProductDeletedAsync(id);
            }
        }


        public async Task NotifyProductAddedAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            // Ensure that necessary properties are not null before proceeding
            if (_productHubContext == null)
            {
                throw new InvalidOperationException("_hubContext is not initialized.");
            }

            if (product.Id == null || product.Name == null)
            {
                throw new InvalidOperationException("Product properties are not initialized.");
            }

            // Logic to notify via SignalR Hub
            await _productHubContext.Clients.All.SendAsync("ProductAdded", product);
        }

        public async Task NotifyProductUpdatedAsync(Product product)
        {
            await _productHubContext.Clients.All.SendAsync("ProductUpdated", product);
        }

        public async Task NotifyProductDeletedAsync(string productId)
        {
            await _productHubContext.Clients.All.SendAsync("ProductDeleted", productId);
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _unitOfWork.Products.SearchProductsAsync(searchTerm);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId)
        {
            return await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
        }



    }
}
