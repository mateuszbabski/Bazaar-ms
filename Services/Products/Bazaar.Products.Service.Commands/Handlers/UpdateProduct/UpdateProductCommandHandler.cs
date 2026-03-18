using Bazaar.Products.Service.Commands.Responses;
using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Commands.Handlers.UpdateProduct
{
    public class UpdateProductCommandHandler
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command) 
        {
            var product = await _productRepository.GetById(command.Id);
            if (product == null)
            {
                return UpdateProductResult.Failure("Product not found.", ErrorCode.ProductNotFound);
            }

            if (command.CustomerId == Guid.Empty || product.CustomerId != command.CustomerId)
            {
                return UpdateProductResult.Failure("Unauthorized to update this product. Product does not belong to the specified customer.", ErrorCode.Forbidden);
            }

            Currency? currency = null;
            if (command.Currency is not null)
            {
                if (!Enum.TryParse<Currency>(command.Currency, ignoreCase: true, out var parsed))
                    return UpdateProductResult.Failure("Invalid currency specified. Specified currency is not allowed.", ErrorCode.ValidationFailed);

                currency = parsed;
            }

            product.Update(
                command.Name,
                command.Description,
                command.Category,
                command.Price,
                currency,
                command.Unit,
                command.WeightPerUnitInKg,
                command.IsAvailable
            );

            await _productRepository.Update(product);

            return UpdateProductResult.Success("Product updated successfully.");
        }
    }
}
