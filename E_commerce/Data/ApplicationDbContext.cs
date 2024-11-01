using E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<product> products { get; set; }
        public DbSet<category> categories { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
                .Build()
                .GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(builder);
        }
    }
}
