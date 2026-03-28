using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Customers.Service.Domain.Models;
using Bazaar.Shared.Infrastructure.Auth;
using NSubstitute;

namespace Bazaar.Customers.Service.Tests.Commands
{
    public class SignUpCommandHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly SignUpCommandHandler _handler;

        public SignUpCommandHandlerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _passwordHasher.GenerateHashPassword(Arg.Any<string>()).Returns("hashed_password");
            _handler = new SignUpCommandHandler(_customerRepository, _passwordHasher);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSuccess()
        {
            _customerRepository.GetByEmail(Arg.Any<string>()).Returns((Customer)null!);

            var command = CreateValidCommand();

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _customerRepository.Received(1).Add(Arg.Any<Customer>());
        }

        [Fact]
        public async Task Handle_WithExistingEmail_ShouldReturnEmailAlreadyRegistered()
        {
            var existingCustomer = Customer.Create(
                "Existing", "User", "john@test.com", "pwd",
                "123", "addr", "city", "00-001", "PL",
                Shared.Models.Models.Role.Customer, null);

            _customerRepository.GetByEmail("john@test.com").Returns(existingCustomer);

            var command = CreateValidCommand();

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.EmailAlreadyRegistered, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithInvalidRole_ShouldReturnValidationFailed()
        {
            _customerRepository.GetByEmail(Arg.Any<string>()).Returns((Customer)null!);

            var command = CreateValidCommand();
            command.Role = "InvalidRole";

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ValidationFailed, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithShopRole_ShouldReturnSuccess()
        {
            _customerRepository.GetByEmail(Arg.Any<string>()).Returns((Customer)null!);

            var command = CreateValidCommand();
            command.Role = "Shop";
            command.ShopName = "Test Shop";

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
        }

        private static SignUpCommand CreateValidCommand() => new()
        {
            Name = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Password = "StrongPassword123!",
            Phone = "123456789",
            Address = "123 Street",
            City = "Warsaw",
            PostalCode = "00-001",
            Country = "Poland",
            Role = "Customer"
        };
    }
}
