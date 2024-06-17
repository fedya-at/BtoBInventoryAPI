using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BtoBInventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImports()
        {
            var imports = await _importService.GetAllImportsAsync();
            return Ok(imports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImportById(string id)
        {
            var import = await _importService.GetImportByIdAsync(id);
            if (import == null)
            {
                return NotFound();
            }
            return Ok(import);
        }

        [HttpPost]
        public async Task<IActionResult> AddImport([FromBody] Import import)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _importService.AddImportAsync(import);
            return CreatedAtAction(nameof(GetImportById), new { id = import.Id }, import);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImport(string id, [FromBody] Import import)
        {
            if (id != import.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingImport = await _importService.GetImportByIdAsync(id);
            if (existingImport == null)
            {
                return NotFound();
            }

            await _importService.UpdateImportAsync(import);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImport(string id)
        {
            var existingImport = await _importService.GetImportByIdAsync(id);
            if (existingImport == null)
            {
                return NotFound();
            }

            await _importService.DeleteImportAsync(id);
            return NoContent();
        }
    }
}
