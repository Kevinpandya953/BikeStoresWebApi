using BikeStoresWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;


namespace BikeStoresWebApi.Data
{
    public class bikeStoresContext:DbContext
    {
        public bikeStoresContext(DbContextOptions<bikeStoresContext> options) :base (options) 
        {
            
        }
        public virtual DbSet<Brands> brand { get; set; }
        public virtual DbSet<Categories> categories { get; set; }

        public virtual DbSet<Products> products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brands>().ToTable("brands", schema: "production");
            modelBuilder.Entity<Categories>().ToTable("categories",schema:"production");
            modelBuilder.Entity<Products>().ToTable("products",schema:"production");
        }
    } 
}
