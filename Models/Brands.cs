using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace BikeStoresWebApi.Models
{
    public class Brands
    {

        [Key]
        [Required]
        public int brand_id { get; set; }
        public string brand_name { get; set; }

        public Brands()
        {
            this.Products = new HashSet<Products>();
        }

        public virtual ICollection<Products> Products { get; set; }
    }
}
