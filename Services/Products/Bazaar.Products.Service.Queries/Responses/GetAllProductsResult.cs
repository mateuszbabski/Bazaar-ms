using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Dtos;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Queries.Responses
{
    public class GetAllProductsResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public List<ProductDto>? Products { get; set; }

        public static GetAllProductsResult Success(List<ProductDto> products, string message)
        {
            return new GetAllProductsResult
            {
                IsSuccess = true,
                Products = products,
                Message = message
            };
        }

        public static GetAllProductsResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new GetAllProductsResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
