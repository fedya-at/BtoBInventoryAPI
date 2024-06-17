using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Repositories
{
    public interface IImportRepository
    {
        Task<IEnumerable<Import>> GetAllImportsAsync();
        Task<Import> GetImportByIdAsync(string id);
        Task AddImportAsync(Import import);
        Task UpdateImportAsync(Import import);
        Task DeleteImportAsync(string id);
    }
}
