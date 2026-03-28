using Bazaar.Customers.Service.Commands.Handlers.SignIn;
using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.Models;
using Bazaar.Shared.Infrastructure.Auth;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Customers.Service.Tests.Commands
{
    public class SignInCommandHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserTokenManager _tokenManager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly SignInCommandHandler _handler;

        public SignInCommandHandlerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _tokenManager = Substitute.For<IUserTokenManager>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _handler = new SignInCommandHandler(_customerRepository, _tokenManager, _passwordHasher);
        }

        [Fact]
        public async Task Handle_WithValidCredentials_ShouldReturnSuccess()
        {
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "hashed_pwd",
                "123", "addr", "city", "00-001", "PL",
                Role.Customer, null);

            _customerRepository.GetByEmail("john@test.com").Returns(customer);
            _passwordHasher.ValidatePassword("password123", "hashed_pwd").Returns(true);
            _tokenManager.GenerateToken(customer.Id, customer.Email, "Customer").Returns("jwt_token");

            var command = new SignInCommand { Email = "john@test.com", Password = "password123" };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            Assert.Equal("jwt_token", result.Token);
            Assert.Equal(customer.Id, result.Id);
        }

        [Fact]
        public async Task Handle_WithNonExistentEmail_ShouldReturnFailure()
        {
            _customerRepository.GetByEmail("unknown@test.com").Returns((Customer)null!);

            var command = new SignInCommand { Email = "unknown@test.com", Password = "password123" };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_WithWrongPassword_ShouldReturnFailure()
        {
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "hashed_pwd",
                "123", "addr", "city", "00-001", "PL",
                Role.Customer, null);

            _customerRepository.GetByEmail("john@test.com").Returns(customer);
            _passwordHasher.ValidatePassword("wrong_password", "hashed_pwd").Returns(false);

            var command = new SignInCommand { Email = "john@test.com", Password = "wrong_password" };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
        }
    }
}
