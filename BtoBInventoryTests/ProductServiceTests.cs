using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Repositories;
using BtoBInventoryAPI.Services;
using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BtoBInventoryTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IHubContext<ProductHub>> _hubContextMock;
        private readonly ProductService _productService;
        private readonly Mock<IClientProxy> _clientProxyMock;


        public ProductServiceTests()
        {
            // Step 1: Create mocks for the dependencies
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _hubContextMock = new Mock<IHubContext<ProductHub>>();
            _clientProxyMock = new Mock<IClientProxy>();


            // Step 2: Set up the UnitOfWork to return the ProductRepository mock
            _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepositoryMock.Object);

            // Step 3: Pass the mocks to the ProductService constructor
            _productService = new ProductService(_unitOfWorkMock.Object, _hubContextMock.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnProductList_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product1", Price = 10 },
                new Product { Id = "2", Name = "Product2", Price = 20 }
            };

            _productRepositoryMock.Setup(repo => repo.GetAllProductsAsync())
                                  .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(products);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = "1";
            var product = new Product { Id = productId, Name = "Product1", Price = 10 };

            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
                                  .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = "nonexistent";

            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
                                  .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddProductAsync_ShouldInvokeRepositoryMethod_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = "3", Name = "NewProduct", Price = 15 };

            // Mocking repository method to ensure it gets called correctly
            _productRepositoryMock.Setup(repo => repo.AddProductAsync(It.IsAny<Product>()))
                                  .Returns(Task.CompletedTask);

            // Mocking the SignalR Hub Context
            _clientProxyMock.Setup(client => client.SendCoreAsync("ProductAdded", new object[] { It.IsAny<Product>() }, default))
                            .Returns(Task.CompletedTask);

            _hubContextMock.Setup(hub => hub.Clients.All).Returns(_clientProxyMock.Object);

            // Act
            await _productService.AddProductAsync(product);

            // Assert
            // Verify that the repository method was called once
            _productRepositoryMock.Verify(repo => repo.AddProductAsync(product), Times.Once);

            // Verify that the hub context's SendCoreAsync method was called once with the correct product
            _clientProxyMock.Verify(client => client.SendCoreAsync("ProductAdded", new object[] { product }, default), Times.Once);
        }



        [Fact]
        public async Task DeleteProductAsync_ShouldInvokeRepositoryMethod_WhenProductExists()
        {
            // Arrange
            var productId = "1";

            _productRepositoryMock.Setup(repo => repo.DeleteProductAsync(productId))
                                  .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
                                  .ReturnsAsync(new Product { Id = productId, Name = "Test Product" });
        }

        [Fact]
        public async Task AddProductAsync_ShouldNotAddProduct_WhenProductIsNull()
        {
            // Act
            Func<Task> act = async () => await _productService.AddProductAsync(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}