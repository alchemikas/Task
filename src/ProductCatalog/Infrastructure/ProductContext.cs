using Microsoft.EntityFrameworkCore;

namespace Product.Api.Infrastructure
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Models.Product>()
                .HasOne(b => b.Image)
                .WithOne();
        }

        public DbSet<Models.Product> Product { get; set; }
        public DbSet<Models.File> File { get; set; }
    }
}
