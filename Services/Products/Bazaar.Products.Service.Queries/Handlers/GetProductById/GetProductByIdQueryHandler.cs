using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Queries.Dtos;
using Bazaar.Products.Service.Queries.Responses;

namespace Bazaar.Products.Service.Queries.Handlers.GetProductById
{
    public class GetProductByIdQueryHandler
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query)
        {
            var product = await _productRepository.GetById(query.Id);
            if (product == null)
            {
                return GetProductByIdResult.Failure("Product not found.", Domain.ErrorCodes.ErrorCode.ProductNotFound);
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                ShopId = product.CustomerId,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                Currency = product.Currency,
                Unit = product.Unit,
                WeightPerUnitInKg = product.WeightPerUnitInKg,
                IsAvailable = product.IsAvailable
            };

            return GetProductByIdResult.Success(productDto, "Product retrieved successfully.");
        }
    }
}
