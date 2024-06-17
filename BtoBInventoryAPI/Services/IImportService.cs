using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Services
{
    public interface IImportService
    {
        Task<IEnumerable<Import>> GetAllImportsAsync();
        Task<Import> GetImportByIdAsync(string id);
        Task AddImportAsync(Import import);
        Task UpdateImportAsync(Import import);
        Task DeleteImportAsync(string id);
    }
}
