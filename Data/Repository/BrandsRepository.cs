using BikeStoresWebApi.Models;

namespace BikeStoresWebApi.Data.Repositories
{
    public class BrandsRepository : Repository<Brands>
    {
        public BrandsRepository(bikeStoresContext context) : base(context)
        {
        }
    }
}
