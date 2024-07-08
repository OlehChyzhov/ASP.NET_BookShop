using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("product_title")]
        public required string Title { get; set; }
        [Column("product_description")]
        public string Description { get; set; }
        [Column("product_isbn")]
        public required string ISBN { get; set; }
        [Column("product_author")]
        public required string Author { get; set; }
        [Range(1, 1000)]
        [Display(Name = "List Price")]
        [Column("product_list_price")]
        public double ListPrice { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Price for 1-50")]
        [Column("price_per_product")]
        public double Price { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Price for 50+")]
        [Column("price_50")]
        public double Price50 { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Price for 100+")]
        [Column("price_100")]
        public double Price100 { get; set; }
    }
}
