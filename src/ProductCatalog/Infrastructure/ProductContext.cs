using System.Linq;
using Microsoft.EntityFrameworkCore;
using Product.Api.DomainCore.Models;

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

            modelBuilder.Entity<DomainCore.Models.Product>()
                .HasOne(b => b.Image)
                .WithOne();

            modelBuilder.Entity<DomainCore.Models.Product>()
                .Property(p => p.Code)
                .HasMaxLength(100)
                .IsRequired();


            modelBuilder.Entity<DomainCore.Models.Product>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<DomainCore.Models.File>()
                .Property(p => p.Title)
                .HasMaxLength(200)
                .IsRequired();

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
            {
                property.Relational().ColumnType = "decimal(5, 2)";
            }
        }

        public DbSet<DomainCore.Models.Product> Product { get; set; }
        public DbSet<File> File { get; set; }
    }
}
