using Bazaar.Customers.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Customers.Service.DataAccess.Context
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomersDbContext(DbContextOptions<CustomersDbContext> opt) 
            : base(opt)
        {          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(c => c.Role)
                .HasConversion<string>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
