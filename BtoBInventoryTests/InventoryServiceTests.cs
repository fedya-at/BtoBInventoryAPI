using BtoBInventoryAPI.Hubs;
using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BtoBInventoryTests
{
    public class InventoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IHubContext<InventoryHub>> _mockHubContext;
        private readonly InventoryService _inventoryService;

        public InventoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHubContext = new Mock<IHubContext<InventoryHub>>();
            _inventoryService = new InventoryService(_mockUnitOfWork.Object, _mockHubContext.Object);
        }

        [Fact]
        public async Task GetAllInventoriesAsync_ReturnsAllInventories()
        {
            // Arrange
            var inventories = new List<Inventory>
            {
                new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 },
                new Inventory { Id = "2", ProductId = "prod2", Quantity = 20 }
            };
            _mockUnitOfWork.Setup(uow => uow.Inventories.GetAllInventoriesAsync())
                           .ReturnsAsync(inventories);

            // Act
            var result = await _inventoryService.GetAllInventoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetInventoryByIdAsync_ValidId_ReturnsInventory()
        {
            // Arrange
            var inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 };
            _mockUnitOfWork.Setup(uow => uow.Inventories.GetInventoryByIdAsync("1"))
                           .ReturnsAsync(inventory);

            // Act
            var result = await _inventoryService.GetInventoryByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("prod1", result.ProductId);
        }

        [Fact]
        public async Task AddInventoryAsync_AddsInventoryAndSendsSignalRMessage()
        {
            // Arrange
            var inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 };
            var product = new Product { Id = "prod1", Name = "Product 1" };

            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

            _mockUnitOfWork.Setup(uow => uow.Products.AddProductAsync(product))
                           .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.Inventories.AddInventoryAsync(inventory))
                           .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                           .Returns(Task.FromResult(0));

            // Act
            await _inventoryService.AddInventoryAsync(inventory, product);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Products.AddProductAsync(product), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Inventories.AddInventoryAsync(inventory), Times.Once);
            mockClientProxy.Verify(client => client.SendCoreAsync("inventoryAdded", new object[] { inventory }, default), Times.Once);
        }

        [Fact]
        public async Task UpdateInventoryAsync_UpdatesInventoryAndSendsSignalRMessage()
        {
            // Arrange
            var inventory = new Inventory { Id = "1", ProductId = "prod1", Quantity = 10 };

            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

            _mockUnitOfWork.Setup(uow => uow.Inventories.UpdateInventoryAsync(inventory))
                           .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                           .Returns(Task.FromResult(0));

            // Act
            await _inventoryService.UpdateInventoryAsync(inventory);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Inventories.UpdateInventoryAsync(inventory), Times.Once);
            mockClientProxy.Verify(client => client.SendCoreAsync("inventoryUpdated", new object[] { inventory }, default), Times.Once);
        }

        [Fact]
        public async Task DeleteInventoryAsync_DeletesInventoryAndSendsSignalRMessage()
        {
            // Arrange
            var inventoryId = "1";

            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            _mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

            _mockUnitOfWork.Setup(uow => uow.Inventories.DeleteInventoryAsync(inventoryId))
                           .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                           .Returns(Task.FromResult(0));

            // Act
            await _inventoryService.DeleteInventoryAsync(inventoryId);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Inventories.DeleteInventoryAsync(inventoryId), Times.Once);
            mockClientProxy.Verify(client => client.SendCoreAsync("inventoryDeleted", new object[] { inventoryId }, default), Times.Once);
        }
    }
}
