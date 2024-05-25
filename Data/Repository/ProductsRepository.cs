using BikeStoresWebApi.Data.Repositories;
using BikeStoresWebApi.Models;

namespace BikeStoresWebApi.Data.Repository
{
    public class ProductsRepository : Repository<Products>
    {
        public ProductsRepository(bikeStoresContext context) : base(context)
        {
        }
    }
}
