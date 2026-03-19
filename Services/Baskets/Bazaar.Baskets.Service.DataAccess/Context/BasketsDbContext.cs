using Bazaar.Baskets.Service.Domain.Models;
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

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
