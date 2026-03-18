using Bazaar.Products.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Products.Service.DataAccess.Context
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ProductsDbContext(DbContextOptions<ProductsDbContext> opt)
            : base(opt)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Currency)
                .HasConversion<string>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
