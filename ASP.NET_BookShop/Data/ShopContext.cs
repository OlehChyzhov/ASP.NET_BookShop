using ASP.NET_BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_BookShop.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options): base(options) {}

        public DbSet<Category> Categories { get; set; }
    }
}
