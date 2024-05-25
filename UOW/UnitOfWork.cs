using BikeStoresWebApi.Data;
using BikeStoresWebApi.Data.Interface;
using BikeStoresWebApi.Data.Repositories;

namespace BikeStoresWebApi.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly bikeStoresContext _context;

        public UnitOfWork(bikeStoresContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            // You can add logic here to return the appropriate repository based on TEntity type
            return new Repository<TEntity>(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
