using System.ComponentModel.DataAnnotations;

namespace BikeStoresWebApi.Models
{
    public class Products
    {
        [Key]
        [Required]
        public int product_id { get; set; }
        public string product_name { get; set; }

        public int brand_id { get; set; }
        public int category_id { get; set; }

         

        public int model_year { get; set; }

        public float list_price { get; set; }

        public virtual Brands Brands { get; set; }

        public virtual Categories Categories { get; set; }
    }
}
