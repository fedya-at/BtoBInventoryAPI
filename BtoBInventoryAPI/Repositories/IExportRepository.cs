using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Repositories
{
    public interface IExportRepository
    {
        Task<IEnumerable<Export>> GetAllExportsAsync();
        Task<Export> GetExportByIdAsync(string id);
        Task AddExportAsync(Export export);
        Task UpdateExportAsync(Export export);
        Task DeleteExportAsync(string id);
    }
}
