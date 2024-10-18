using BtoBInventoryAPI.Controllers;
using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BtoBInventoryTests
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryServices> _mockInventoryService;
        private readonly InventoryController _controller;

        public InventoryControllerTests()
        {
            _mockInventoryService = new Mock<IInventoryServices>();
            _controller = new InventoryController(_mockInventoryService.Object);
        }

        [Fact]
        public async Task GetAllInventories_ReturnsOkResult_WithListOfInventories()
        {
            // Arrange
            var inventories = new List<Inventory>
            {
                new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 },
                new Inventory { Id = "2", ProductId = "prod2", Quantity = 20 }
            };
            _mockInventoryService.Setup(service => service.GetAllInventoriesAsync())
                                 .ReturnsAsync(inventories);

            // Act
            var result = await _controller.GetAllInventories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnInventories = Assert.IsType<List<Inventory>>(okResult.Value);
            Assert.Equal(2, returnInventories.Count);
        }

        [Fact]
        public async Task GetInventoryById_ExistingId_ReturnsOkResult_WithInventory()
        {
            // Arrange
            var inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 };
            _mockInventoryService.Setup(service => service.GetInventoryByIdAsync("1"))
                                 .ReturnsAsync(inventory);

            // Act
            var result = await _controller.GetInventoryById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnInventory = Assert.IsType<Inventory>(okResult.Value);
            Assert.Equal("1", returnInventory.Id);
        }

        [Fact]
        public async Task GetInventoryById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            _mockInventoryService.Setup(service => service.GetInventoryByIdAsync("1"))
                                 .ReturnsAsync((Inventory)null);

            // Act
            var result = await _controller.GetInventoryById("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddInventory_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var request = new InventoryProductRequest
            {
                Inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 },
                Product = new Product { Id = "prod1", Name = "Product 1" }
            };

            // Act
            var result = await _controller.AddInventory(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetInventoryById", createdAtActionResult.ActionName);
            Assert.Equal("1", ((Inventory)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateInventory_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 15 };
            _mockInventoryService.Setup(service => service.GetInventoryByIdAsync("1"))
                                 .ReturnsAsync(inventory);
            _mockInventoryService.Setup(service => service.UpdateInventoryAsync(inventory))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateInventory("1", inventory);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteInventory_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 };
            _mockInventoryService.Setup(service => service.GetInventoryByIdAsync("1"))
                                 .ReturnsAsync(inventory);
            _mockInventoryService.Setup(service => service.DeleteInventoryAsync("1"))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteInventory("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}