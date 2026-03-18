using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Dtos;
using Bazaar.Products.Service.Queries.Responses;

namespace Bazaar.Products.Service.Queries.Handlers.SearchProducts
{
    public class SearchProductsQueryHandler
    {
        private readonly IProductRepository _productRepository;

        public SearchProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<SearchProductsResult> Handle(SearchProductsQuery query)
        {
            var products = await _productRepository.SearchProducts(
                query.Name,
                query.Category,
                query.MinPrice,
                query.MaxPrice);

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                ShopId = p.CustomerId,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                Price = p.Price,
                Currency = p.Currency,
                Unit = p.Unit,
                WeightPerUnitInKg = p.WeightPerUnitInKg,
                IsAvailable = p.IsAvailable
            }).ToList();

            if (productDtos.Count == 0)
                return SearchProductsResult.Failure("Products not found", ErrorCode.ProductsNotFound);

            return SearchProductsResult.Success(productDtos, "Products retrieved successfully.");
        }
    }
}
