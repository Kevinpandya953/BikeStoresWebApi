using BikeStoresWebApi.Data.Interface;

namespace BikeStoresWebApi.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task SaveAsync();
    }
}
