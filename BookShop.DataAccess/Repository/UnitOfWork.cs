using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShopContext shop_context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserExtentionRepository ExtentionUser { get; private set; }

        public UnitOfWork(ShopContext context)
        {
            shop_context = context;
            Category = new CategoryRepository(shop_context);
            Product = new ProductRepository(shop_context);
            Company = new CompanyRepository(shop_context);
            ShoppingCart = new ShoppingCartRepository(shop_context);
            ExtentionUser = new UserExtentionRepository(shop_context);
        }
        public void SaveChanges()
        {
            shop_context.SaveChanges();
        }
    }
}
