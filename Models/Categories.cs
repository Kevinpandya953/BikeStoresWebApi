using System.ComponentModel.DataAnnotations;

namespace BikeStoresWebApi.Models
{
    public class Categories
    {
        [Key]
        [Required]
        public int category_id { get; set;}

        public string category_name { get; set;}

        public Categories()
        {
            this.Products = new HashSet<Products>();
        }

        public ICollection<Products> Products { get; set; }
    }
}
