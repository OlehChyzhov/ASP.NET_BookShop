using BookShop.DataAccess.Repository.IRepository;
using BookShop.DataAccess.Data;
using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ShopContext shop_context;
        public OrderHeaderRepository(ShopContext context) : base(context) 
        { 
            shop_context = context; 
        }

        public void Update(OrderHeader order_header)
        {
            shop_context.OrderHeaders.Update(order_header);
        }
    }
}
