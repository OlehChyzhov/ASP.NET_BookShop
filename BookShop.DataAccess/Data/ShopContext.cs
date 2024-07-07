using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.DataAcesss.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options): base(options) {}

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Category[] default_categories = 
            {
                new Category() { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category() { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category() {Id = 3, Name = "History", DisplayOrder = 3},
            };
            modelBuilder.Entity<Category>().HasData(default_categories);
        }
    }
}
