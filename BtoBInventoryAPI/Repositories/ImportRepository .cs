using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Models;
using MongoDB.Driver;

namespace BtoBInventoryAPI.Repositories
{
    public class ImportRepository : IImportRepository
    {
        private readonly IDatabaseContext _context;

    public ImportRepository(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Import>> GetAllImportsAsync()
    {
        var cursor = await _context.Imports.FindAsync(Builders<Import>.Filter.Empty);
        return await cursor.ToListAsync();
    }

    public async Task<Import> GetImportByIdAsync(string id)
    {
        return await _context.Imports.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddImportAsync(Import import)
    {
        await _context.Imports.InsertOneAsync(import);
    }

    public async Task UpdateImportAsync(Import import)
    {
        var filter = Builders<Import>.Filter.Eq(i => i.Id, import.Id);
        await _context.Imports.ReplaceOneAsync(filter, import);
    }

    public async Task DeleteImportAsync(string id)
    {
        var filter = Builders<Import>.Filter.Eq(i => i.Id, id);
        await _context.Imports.DeleteOneAsync(filter);
    }
}
}
