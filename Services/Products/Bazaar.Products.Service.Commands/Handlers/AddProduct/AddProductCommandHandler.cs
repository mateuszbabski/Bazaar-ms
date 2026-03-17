using Bazaar.Products.Service.Commands.Responses;
using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Domain.Models;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Commands.Handlers.AddProduct
{
    public class AddProductCommandHandler
    {
        private readonly IProductRepository _productRepository;

        public AddProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<AddProductCommandResult> Handle(AddProductCommand command)
        {
            var validator = new AddProductCommandValidator();
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return AddProductCommandResult.Failure("Validation failed.", ErrorCode.ValidationFailed);
            }

            if (!Enum.TryParse<Currency>(command.Currency, ignoreCase: true, out var currency))
            {
                return AddProductCommandResult.Failure("Invalid currency specified. Specified currency is not allowed.", ErrorCode.ValidationFailed);
            }

            var product = Product.Create(
                command.CustomerId,
                command.Name,
                command.Description,
                command.Category,
                command.Price,
                currency,
                command.Unit,
                command.WeightPerUnitInKg,
                command.IsAvailable
            );

            await _productRepository.Add(product);

            return AddProductCommandResult.Success("Product added successfully.");
        }            
    }
}
