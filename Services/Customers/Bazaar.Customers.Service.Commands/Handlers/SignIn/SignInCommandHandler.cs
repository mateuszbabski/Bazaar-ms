using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Customers.Service.Commands.Handlers.SignIn
{
    public class SignInCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserTokenManager _tokenManager;
        private readonly IPasswordHasher _passwordHasher;

        public SignInCommandHandler(
            ICustomerRepository customerRepository, 
            IUserTokenManager tokenManager, 
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
                return AuthenticationResult.Failure("Invalid email or password.");
            }

            var token = _tokenManager.GenerateToken(customer.Id, customer.Email, customer.Role);

            return AuthenticationResult.Success(customer.Id, token);            
        }
    }
}
