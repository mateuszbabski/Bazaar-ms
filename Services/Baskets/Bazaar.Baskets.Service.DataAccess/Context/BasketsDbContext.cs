using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Baskets.Service.DataAccess.Context
{
    internal class BasketsDbContext : DbContext
    {
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        public BasketsDbContext(DbContextOptions<BasketsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>()
                .Property(b => b.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<BasketItem>()
                .Property(i => i.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<BasketItem>()
                .Property(i => i.BaseCurrency)
                .HasConversion<string>();

            modelBuilder.Entity<BasketItem>()
                .HasOne(i => i.Basket)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.BasketId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
