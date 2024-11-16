using E_commerce.Models;
using System.Linq.Expressions;

namespace E_commerce.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        List<T> GatAll(string? include = null);

        // CRUD
        public IEnumerable<T> Get(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null);

        T? GetOne(Expression<Func<T, bool>> expression);

        T? GetById(int entityId);


        void create(T entity);



        void Edit(T entity);



        void Delete(T entity);


        void commit();
    }
}
