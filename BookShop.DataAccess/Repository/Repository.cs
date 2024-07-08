using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ShopContext shop_context;
        DbSet<T> Table;
        public Repository(ShopContext context)
        {
            shop_context = context;
            this.Table = shop_context.Set<T>();
        }
        public void Add(T entity)
        {
            Table.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = Table;
            return query.ToList();
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = Table;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            Table.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Table.RemoveRange(entities);
        }
    }
}
