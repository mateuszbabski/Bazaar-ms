using Bazaar.Customers.Service.DataAccess.Context;
using Bazaar.Customers.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Customers.Service.DataAccess.Repository
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly CustomersDbContext _context;

        public CustomerRepository(CustomersDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task Delete(Guid id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> Get(Guid id) 
            => await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Customer> GetByEmail(string email) 
            => await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

        public async Task Update(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
