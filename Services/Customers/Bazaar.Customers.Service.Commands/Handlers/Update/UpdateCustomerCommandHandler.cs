using Bazaar.Customers.Service.Commands.Responses;
using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.ErrorCodes;

namespace Bazaar.Customers.Service.Commands.Handlers.Update
{
    public class UpdateCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand command)
        {
            var customer = await _customerRepository.Get(command.Id.Value);
            if (customer == null)
            {
                return UpdateCustomerResult.Failure("Customer not found.", ErrorCode.CustomerNotFound);
            }

            var validator = new UpdateValidator();
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid) {
                return UpdateCustomerResult.Failure("Validation failed.", ErrorCode.ValidationFailed);
            }

            customer.Update(
                command.Name, 
                command.LastName, 
                command.Phone, 
                command.Address, 
                command.City, 
                command.PostalCode, 
                command.Country);

            await _customerRepository.Update(customer);
            return UpdateCustomerResult.Success("Customer updated successfully.");
        }
    }
}
