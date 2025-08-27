using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVCPractice.Models
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } 
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.OrderHeader)
                .WithMany(oh => oh.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Cascade delete বন্ধ

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // ShoppingCart relations
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.Product)
                .WithMany()
                .HasForeignKey(sc => sc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.ApplcationUsers)
                .WithMany()
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderHeader → User
            modelBuilder.Entity<OrderHeader>()
                .HasOne(oh => oh.ApplicationUser)
                .WithMany()
                .HasForeignKey(oh => oh.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}
    