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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderTracking> OrderTracking { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
                .Build()
                .GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(builder);
        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // العلاقة بين Order و OrderDetail
    modelBuilder.Entity<OrderDetail>()
        .HasOne(od => od.Order)
        .WithMany(o => o.OrderDetails)
        .HasForeignKey(od => od.OrderId);

    // العلاقة بين Order و OrderTracking
    modelBuilder.Entity<OrderTracking>()
        .HasOne(ot => ot.Order)
        .WithMany(o => o.OrderTrackingDetails)
        .HasForeignKey(ot => ot.OrderId);
}

        
    }
}
