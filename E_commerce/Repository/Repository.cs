
using E_commerce.Data;
using E_commerce.Migrations;
using E_commerce.Models;
using E_commerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_commerce.Repository
{
    public class Repository<T> : IRepository<T> where T : class

    {

        //ApplicationDbContext dbContext = new ApplicationDbContext();
        private readonly ApplicationDbContext dbContext;
        private DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public List<T> GatAll(string? include = null)
        {
            return include == null ? dbSet.ToList() : dbSet.Include(include).ToList();
        }

        // CRUD operations
        public IEnumerable<T> Get(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>> expression = null)
        {
            IQueryable<T> query = dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includeProp != null)

            {
                foreach(var prop in includeProp)
                {
                    query = query.Include(prop);

                }
            }

            return query.ToList();
        }

        public T? GetOne(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).FirstOrDefault();
        }


        public T? GetById(int entityId)
        {
            return dbSet.Find(entityId);
        }

        public void create(T entity)
        {
            dbSet.Add(entity);
        }

        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void commit()
        {
            dbContext.SaveChanges();

        }
    }
}
