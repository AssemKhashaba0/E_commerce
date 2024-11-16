using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository.IRepository;

namespace E_commerce.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
