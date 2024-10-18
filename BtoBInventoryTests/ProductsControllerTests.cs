using BtoBInventoryAPI.Controllers;
using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtoBInventoryTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithProductList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product1", Price = 10 },
                new Product { Id = "2", Name = "Product2", Price = 20 }
            };
            _productServiceMock.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count);
        }

        [Fact]
        public async Task GetProductById_ValidId_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var validId = ObjectId.GenerateNewId().ToString(); // Generate a valid ObjectId string
            var expectedProduct = new Product { Id = validId, Name = "Test Product" }; // Create a product to return

            // Mock the service to return the expected product when called with the valid ID
            mockProductService.Setup(service => service.GetProductByIdAsync(validId))
                .ReturnsAsync(expectedProduct);

            var controller = new ProductsController(mockProductService.Object);

            // Act
            var result = await controller.GetProductById(validId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(expectedProduct.Id, returnedProduct.Id);
            Assert.Equal(expectedProduct.Name, returnedProduct.Name);
        }

        [Fact]
        public async Task GetProductById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var controller = new ProductsController(mockProductService.Object);

            string invalidId = "invalid_object_id"; // Example of an invalid ObjectId format

            // Act
            var result = await controller.GetProductById(invalidId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid ObjectId format", badRequestResult.Value);
        }

        [Fact]
        public async Task AddProduct_ValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Product1", Price = 10 };
            _productServiceMock.Setup(service => service.AddProductAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddProduct(product);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetProductById", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task UpdateProduct_ValidProduct_ReturnsNoContent()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "UpdatedProduct", Price = 15 };
            _productServiceMock.Setup(service => service.GetProductByIdAsync("1")).ReturnsAsync(product);
            _productServiceMock.Setup(service => service.UpdateProductAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct("1", product);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ValidId_ReturnsNoContent()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var productId = ObjectId.GenerateNewId().ToString();
            var product = new Product { Id = productId, Name = "Test Product", Price = 10.0m };

            // Mocking the product retrieval to return a product
            mockProductService.Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            // Mocking the deletion
            mockProductService.Setup(service => service.DeleteProductAsync(productId))
                .Returns(Task.CompletedTask);

            var controller = new ProductsController(mockProductService.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }


        // Add more tests for other actions (GetProductsByCriteria, SearchProducts, etc.)
    }
}
