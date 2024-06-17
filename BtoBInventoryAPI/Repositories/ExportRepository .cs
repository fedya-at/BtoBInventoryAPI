using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Models;
using MongoDB.Driver;

namespace BtoBInventoryAPI.Repositories
{
    public class ExportRepository : IExportRepository
    {
        private readonly IDatabaseContext _context;

        public ExportRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Export>> GetAllExportsAsync()
        {
            var cursor = await _context.Exports.FindAsync(Builders<Export>.Filter.Empty);
            return await cursor.ToListAsync();
        }

        public async Task<Export> GetExportByIdAsync(string id)
        {
            return await _context.Exports.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddExportAsync(Export export)
        {
            await _context.Exports.InsertOneAsync(export);
        }

        public async Task UpdateExportAsync(Export export)
        {
            var filter = Builders<Export>.Filter.Eq(e => e.Id, export.Id);
            await _context.Exports.ReplaceOneAsync(filter, export);
        }

        public async Task DeleteExportAsync(string id)
        {
            var filter = Builders<Export>.Filter.Eq(e => e.Id, id);
            await _context.Exports.DeleteOneAsync(filter);
        }
    }

}