using BtoBInventoryAPI.Models;

namespace BtoBInventoryAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _unitOfWork.Categories.GetCategoryByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.AddCategoryAsync(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.UpdateCategoryAsync(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _unitOfWork.Categories.DeleteCategoryAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
