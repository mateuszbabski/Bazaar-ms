using Bazaar.Customers.Service.Commands.Handlers.Update;
using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Customers.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Customers.Service.Tests.Commands
{
    public class UpdateCustomerCommandHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UpdateCustomerCommandHandler _handler;

        public UpdateCustomerCommandHandlerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _handler = new UpdateCustomerCommandHandler(_customerRepository);
        }

        [Fact]
        public async Task Handle_WithExistingCustomer_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "pwd",
                "123", "addr", "city", "00-001", "PL",
                Role.Customer, null);
            customer.Id = customerId;

            _customerRepository.Get(customerId).Returns(customer);

            var command = new UpdateCustomerCommand
            {
                Id = customerId,
                Name = "Jane",
                City = "Cracow"
            };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            Assert.Equal("Jane", customer.Name);
            Assert.Equal("Cracow", customer.City);
            await _customerRepository.Received(1).Update(customer);
        }

        [Fact]
        public async Task Handle_WithNonExistentCustomer_ShouldReturnCustomerNotFound()
        {
            var customerId = Guid.NewGuid();
            _customerRepository.Get(customerId).Returns((Customer)null!);

            var command = new UpdateCustomerCommand { Id = customerId, Name = "Jane" };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.CustomerNotFound, result.ErrorCode);
        }
    }
}
