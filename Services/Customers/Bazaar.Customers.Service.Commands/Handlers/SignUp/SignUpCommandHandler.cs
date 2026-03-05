using Bazaar.Customers.Service.Commands.Responses;
using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.Models;
using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Customers.Service.Commands.Handlers.SignUp
{
    public class SignUpCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;

        public SignUpCommandHandler(
            ICustomerRepository customerRepository,
            IPasswordHasher passwordHasher)
        {
            _customerRepository = customerRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<SignUpResult> Handle(SignUpCommand command)
        {
            var existingCustomer = await _customerRepository.Get(command.Email);
            if (existingCustomer is not null)
            {
                return SignUpResult.Failure("Email is already registered.", ErrorCode.EmailAlreadyRegistered);
            }
            
            var validator = new SignUpValidator();
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return SignUpResult.Failure(
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), 
                    ErrorCode.ValidationFailed);
            }

            var hashedPassword = _passwordHasher.GenerateHashPassword(command.Password);
            var customer = Customer.Create(
                command.Name, 
                command.LastName,
                command.Email,
                hashedPassword,
                command.Phone,
                command.Address,
                command.City,
                command.PostalCode,
                command.Country);

            await _customerRepository.Add(customer);
            
            return SignUpResult.Success("Customer registered successfully.");
        }
    }
}
