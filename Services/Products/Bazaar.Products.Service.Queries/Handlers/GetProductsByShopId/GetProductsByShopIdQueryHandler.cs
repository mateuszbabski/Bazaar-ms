using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Dtos;
using Bazaar.Products.Service.Queries.Responses;

namespace Bazaar.Products.Service.Queries.Handlers.GetProductsByShopId
{
    public class GetProductsByShopIdQueryHandler
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByShopIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetAllProductsResult> Handle(GetProductsByShopIdQuery query)
        {
            try
            {
                var products = await _productRepository.GetProductsByShopId(query.ShopId);
                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    ShopId = p.CustomerId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Currency = p.Currency,
                    Unit = p.Unit,
                    WeightPerUnitInKg = p.WeightPerUnitInKg,
                    IsAvailable = p.IsAvailable
                }).ToList();
                if (productDtos.Count == 0)
                    return GetAllProductsResult.Failure("Products not found", ErrorCode.ProductsNotFound);

                return GetAllProductsResult.Success(productDtos, "Products retrieved successfully.");
            }
            catch (Exception ex)
            {
                return GetAllProductsResult.Failure($"An error occurred while retrieving products. Error details: #{ex.Message}", ErrorCode.UnknownError);
            }
        }
    }
}
