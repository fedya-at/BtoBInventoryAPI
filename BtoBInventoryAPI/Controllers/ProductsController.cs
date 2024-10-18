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
        [HttpGet("sorted")]
        public async Task<IActionResult> GetProductsByCriteria(string sortBy, string sortOrder)
        {
            var products = await _productService.GetAllProductsAsync();

            switch (sortBy?.ToLower())
            {
                case "name":
                    products = sortOrder?.ToLower() == "desc" ?
                        products.OrderByDescending(p => p.Name).ToList() :
                        products.OrderBy(p => p.Name).ToList();
                    break;
                case "price":
                    products = sortOrder?.ToLower() == "desc" ?
                        products.OrderByDescending(p => p.Price).ToList() :
                        products.OrderBy(p => p.Price).ToList();
                    break;
                case "categoryname": // Assuming you want to sort by categoryName
                    products = sortOrder?.ToLower() == "desc" ?
                        products.OrderByDescending(p => p.Category.NameCategory).ToList() :
                        products.OrderBy(p => p.Category.NameCategory).ToList();
                    break;

                default:
                    break;
            }

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

        [HttpGet("{id}/price")]
        public async Task<IActionResult> GetProductPriceById(string id)
        {
            ObjectId productId;
            if (!ObjectId.TryParse(id, out productId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var price = await _productService.GetProductPriceByIdAsync(productId.ToString());
            return Ok(price);
           
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
       
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Log the file path for debugging
            var imageUrl = $"/Images/{uniqueFileName}";
            Console.WriteLine($"Image URL: {imageUrl}");

            return Ok(new { imageUrl });
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }


    }
}
