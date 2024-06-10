using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace BtoBInventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            ObjectId productId;
            if (!ObjectId.TryParse(id, out productId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var product = await _productService.GetProductByIdAsync(productId.ToString());
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(product.Id))
            {
                product.Id = ObjectId.GenerateNewId().ToString();
            }


            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id.ToString() }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product product)
        {
            if (id != product.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            ObjectId productId;
            if (!ObjectId.TryParse(id, out productId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var existingProduct = await _productService.GetProductByIdAsync(productId.ToString());
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(productId.ToString());
            return NoContent();
        }
    }
}
