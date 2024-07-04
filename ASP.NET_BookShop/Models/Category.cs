using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_BookShop.Models
{
    [Table("category")]
    public class Category
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("display_order")]
        public int DisplayOrder { get; set; }
    }
}
