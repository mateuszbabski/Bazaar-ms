using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Customers.Service.Domain.Models;
using Bazaar.Customers.Service.Queries.Handlers;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Customers.Service.Tests.Queries
{
    public class GetCustomerQueryHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly GetCustomerQueryHandler _handler;

        public GetCustomerQueryHandlerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _handler = new GetCustomerQueryHandler(_customerRepository);
        }

        [Fact]
        public async Task Handle_WithExistingCustomer_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "pwd",
                "123456789", "123 Street", "Warsaw", "00-001", "Poland",
                Role.Customer, null);
            customer.Id = customerId;

            _customerRepository.Get(customerId).Returns(customer);

            var result = await _handler.Handle(customerId.ToString());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Customer);
            Assert.Equal("John", result.Customer.Name);
            Assert.Equal("john@test.com", result.Customer.Email);
        }

        [Fact]
        public async Task Handle_WithNonExistentCustomer_ShouldReturnCustomerNotFound()
        {
            var customerId = Guid.NewGuid();
            _customerRepository.Get(customerId).Returns((Customer)null!);

            var result = await _handler.Handle(customerId.ToString());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.CustomerNotFound, result.ErrorCode);
        }
    }
}
