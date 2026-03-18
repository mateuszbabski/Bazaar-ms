using Bazaar.Customers.Service.Commands.Responses;
using Bazaar.Customers.Service.DataAccess.Repository;
using Bazaar.Customers.Service.Domain.ErrorCodes;

namespace Bazaar.Customers.Service.Commands.Handlers.UpdateToShop
{
    public class UpdateToShopCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateToShopCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<UpdateCustomerResult> Handle(UpdateToShopCommand command)
        {
            var customer = await _customerRepository.Get(command.Id);
            if (customer == null)
            {
                return UpdateCustomerResult.Failure("Customer not found.", ErrorCode.CustomerNotFound);
            }

            var validator = new UpdateToShopValidator();
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return UpdateCustomerResult.Failure(
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    ErrorCode.ValidationFailed);
            }

            customer.UpdateToShop(command.ShopName);

            await _customerRepository.Update(customer);
            return UpdateCustomerResult.Success("Customer updated successfully.");
        }
    }
}
