using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Dtos;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Queries.Responses
{
    public class SearchProductsResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public List<ProductDto>? Products { get; set; }

        public static SearchProductsResult Success(List<ProductDto> products, string message)
        {
            return new SearchProductsResult
            {
                IsSuccess = true,
                Products = products,
                Message = message
            };
        }

        public static SearchProductsResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new SearchProductsResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
