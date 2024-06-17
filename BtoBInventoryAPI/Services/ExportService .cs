using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Services
{
    public class ExportService : IExportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Export>> GetAllExportsAsync()
        {
            return await _unitOfWork.Exports.GetAllExportsAsync();
        }

        public async Task<Export> GetExportByIdAsync(string id)
        {
            return await _unitOfWork.Exports.GetExportByIdAsync(id);
        }

        public async Task AddExportAsync(Export export)
        {
            await _unitOfWork.Exports.AddExportAsync(export);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateExportAsync(Export export)
        {
            await _unitOfWork.Exports.UpdateExportAsync(export);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteExportAsync(string id)
        {
            await _unitOfWork.Exports.DeleteExportAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
