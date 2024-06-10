using BtoBInventoryAPI.Repositories;

namespace BtoBInventoryAPI.Services
{

    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IInventoryRepository Inventories { get; }

        Task<int> CompleteAsync();
    }

}
