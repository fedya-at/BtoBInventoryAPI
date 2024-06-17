using BtoBInventoryAPI.Models;
using BtoBInventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BtoBInventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _exportService;

        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExports()
        {
            var exports = await _exportService.GetAllExportsAsync();
            return Ok(exports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExportById(string id)
        {
            var export = await _exportService.GetExportByIdAsync(id);
            if (export == null)
            {
                return NotFound();
            }
            return Ok(export);
        }

        [HttpPost]
        public async Task<IActionResult> AddExport([FromBody] Export export)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _exportService.AddExportAsync(export);
            return CreatedAtAction(nameof(GetExportById), new { id = export.Id }, export);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExport(string id, [FromBody] Export export)
        {
            if (id != export.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingExport = await _exportService.GetExportByIdAsync(id);
            if (existingExport == null)
            {
                return NotFound();
            }

            await _exportService.UpdateExportAsync(export);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExport(string id)
        {
            var existingExport = await _exportService.GetExportByIdAsync(id);
            if (existingExport == null)
            {
                return NotFound();
            }

            await _exportService.DeleteExportAsync(id);
            return NoContent();
        }
    }
}
