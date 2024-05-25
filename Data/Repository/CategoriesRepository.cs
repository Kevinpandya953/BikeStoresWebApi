using BikeStoresWebApi.Data.Repositories;
using BikeStoresWebApi.Models;

namespace BikeStoresWebApi.Data.Repository
{
    public class CategoriesRepository : Repository<Categories>
    {
        public CategoriesRepository(bikeStoresContext context) : base(context)
        {
        }
    }
}
