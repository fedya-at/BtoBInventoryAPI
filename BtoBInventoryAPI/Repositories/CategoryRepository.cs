using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Models;
using MongoDB.Driver;

namespace BtoBInventoryAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDatabaseContext _context;

        public CategoryRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var cursor = await _context.Categories.FindAsync(Builders<Category>.Filter.Empty);
            return await cursor.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _context.Categories.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.InsertOneAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);
            await _context.Categories.ReplaceOneAsync(filter, category);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, id);
            await _context.Categories.DeleteOneAsync(filter);
        }
    }
}