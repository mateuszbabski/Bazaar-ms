using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Customers.Service.Commands.Handlers.SignIn
{
    public class SignInCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITokenManager _tokenManager;
        private readonly IPasswordHasher _passwordHasher;

        public SignInCommandHandler(
            ICustomerRepository customerRepository, 
            ITokenManager tokenManager, 
            IPasswordHasher passwordHasher)
        {
            _customerRepository = customerRepository;
            _tokenManager = tokenManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticationResult> Handle(SignInCommand command)
        {
            var customer = await _customerRepository.Get(command.Email);
            if (customer == null || !_passwordHasher.ValidatePassword(command.Password, customer.PasswordHash))
            {
                return new AuthenticationResult
                {
                    Id = Guid.Empty,
                    Token = string.Empty,
                    Status = "Failure",
                    Message = "Invalid email or password."
                };
            }

            var token = _tokenManager.GenerateToken(customer.Id, customer.Email, customer.Role);
            return new AuthenticationResult
            {
                Token = token,
                Id = customer.Id,
                Status = "Success"
            };
        }
    }
}
