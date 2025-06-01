using Microsoft.EntityFrameworkCore;
using NhaHang.Models;

namespace NhaHang.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Menu> Menu { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }

        public DbSet<DatBan> DatBan { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderItem)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
