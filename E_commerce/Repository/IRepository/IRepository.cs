using E_commerce.Models;

namespace E_commerce.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        List<T> GatAll(string? include = null);


        T? GetById(int entityId);


        void create(T entity);



        void Edit(T entity);



        void Delete(T entity);


        void commit();
    }
}
