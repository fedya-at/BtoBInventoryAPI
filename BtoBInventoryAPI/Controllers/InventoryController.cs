using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryServices _inventoryService;

        public InventoryController(IInventoryServices inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventories()
        {
            var inventories = await _inventoryService.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(string id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }
        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] InventoryProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _inventoryService.AddInventoryAsync(request.Inventory, request.Product);
            return CreatedAtAction(nameof(GetInventoryById), new { id = request.Inventory.Id }, request.Inventory);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(string id, [FromBody] Inventory inventory)
        {
            if (id != inventory.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingInventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (existingInventory == null)
            {
                return NotFound();
            }

            await _inventoryService.UpdateInventoryAsync(inventory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(string id)
        {
            var existingInventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (existingInventory == null)
            {
                return NotFound();
            }

            await _inventoryService.DeleteInventoryAsync(id);
            return NoContent();
        }

        
    }

}
