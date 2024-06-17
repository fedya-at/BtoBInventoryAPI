using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Services
{
    public class ImportService : IImportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Import>> GetAllImportsAsync()
        {
            return await _unitOfWork.Imports.GetAllImportsAsync();
        }

        public async Task<Import> GetImportByIdAsync(string id)
        {
            return await _unitOfWork.Imports.GetImportByIdAsync(id);
        }

        public async Task AddImportAsync(Import import)
        {
            await _unitOfWork.Imports.AddImportAsync(import);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateImportAsync(Import import)
        {
            await _unitOfWork.Imports.UpdateImportAsync(import);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteImportAsync(string id)
        {
            await _unitOfWork.Imports.DeleteImportAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
