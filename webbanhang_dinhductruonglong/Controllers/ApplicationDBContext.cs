
using Microsoft.EntityFrameworkCore;
using webbanhang_dinhductruonglong.Models;

namespace webbanhang_dinhductruonglong.Controllers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
       public DbSet<User> Users { get; set; }
        public DbSet <Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ 1 ShoppingCart -> n CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.ShoppingCart)
                .WithMany(s => s.Items)
                .HasForeignKey(c => c.ShoppingCartId);
        }
     

    }

}
