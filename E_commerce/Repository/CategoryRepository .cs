using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repository
{
    public class CategoryRepository : Repository<category> , ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) 
        {
            this.dbContext = dbContext;
        }
     
       
    }
}
