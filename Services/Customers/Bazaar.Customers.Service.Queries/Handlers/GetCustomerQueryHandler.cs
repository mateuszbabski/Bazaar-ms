using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Customers.Service.Queries.Dtos;
using Bazaar.Customers.Service.Queries.Responses;

namespace Bazaar.Customers.Service.Queries.Handlers
{
    public class GetCustomerQueryHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomerResult> Handle(string userId)
        {
            var customerGuidId = Guid.Parse(userId);
            var customer = await _customerRepository.Get(customerGuidId);
            if (customer == null)
            {
                return GetCustomerResult.Failure("Customer not found.", ErrorCode.CustomerNotFound);
            }

            return GetCustomerResult.Success(
                new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address,
                    City = customer.City,
                    PostalCode = customer.PostalCode,
                    Country = customer.Country
                }, 
                message: "Customer's data fetched successfully.");
        }
    }
}
