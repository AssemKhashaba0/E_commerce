using E_commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_commerce.ViewModel;

namespace E_commerce.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
            {
            }

        public ApplicationDbContext()
        {
            
        }
        public DbSet<product> products { get; set; }
        public DbSet<category> categories { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        public DbSet<Campany> Campanies { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
                .Build()
                .GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(builder);
        }
        public DbSet<E_commerce.ViewModel.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<E_commerce.ViewModel.LoginVM> LoginVM { get; set; } = default!;
    }
}
