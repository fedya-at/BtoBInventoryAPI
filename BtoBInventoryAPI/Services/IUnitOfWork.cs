using BtoBInventoryAPI.Repositories;

namespace BtoBInventoryAPI.Services
{

    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IInventoryRepository Inventories { get; }

        IExportRepository Exports { get; }
        IImportRepository Imports { get; }

        ICategoryRepository Categories { get; }


        Task<int> CompleteAsync();
    }

}
