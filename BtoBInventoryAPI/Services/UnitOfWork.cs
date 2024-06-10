using BtoBInventoryAPI.Data;
using BtoBInventoryAPI.Repositories;

namespace BtoBInventoryAPI.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContext _context;
        public IProductRepository Products { get; private set; }
        public IInventoryRepository Inventories { get; private set; }

        public UnitOfWork(IDatabaseContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Inventories = new InventoryRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await Task.FromResult(0);
        }

        public void Dispose()
        {
        }
    }
}
