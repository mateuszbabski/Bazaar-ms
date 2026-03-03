using Bazaar.Customers.Service.Domain.Models;

namespace Bazaar.Customers.Service.DataAccess.Repository
{
    public interface ICustomerRepository
    {
        Task<Customer> Add(Customer customer);
        Task Update(Customer customer);
        Task Delete(Guid id);
        Task<Customer> Get(Guid id);
        Task<Customer> Get(string email);
    }
}
