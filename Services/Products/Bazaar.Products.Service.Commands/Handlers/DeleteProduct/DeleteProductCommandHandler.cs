using Bazaar.Products.Service.Commands.Responses;
using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;

namespace Bazaar.Products.Service.Commands.Handlers.DeleteProduct
{
    public class DeleteProductCommandHandler
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command)
        {
            var product = await _productRepository.GetById(command.Id);
            if (product == null)
                return new DeleteProductResult
                {
                    IsSuccess = false,
                    ErrorCode = ErrorCode.ProductNotFound,
                    ErrorMessage = "Product not found"
                };

            if (product.CustomerId != command.CustomerId)
                return new DeleteProductResult
                {
                    IsSuccess = false,
                    ErrorCode = ErrorCode.Forbidden,
                    ErrorMessage = "You do not have permission to delete this product"
                };

            await _productRepository.Delete(product);
            return new DeleteProductResult { IsSuccess = true, Message = "Product successfully deleted." };
        }
    }
}
