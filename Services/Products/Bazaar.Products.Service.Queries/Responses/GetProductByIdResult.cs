using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Dtos;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Queries.Responses
{
    public class GetProductByIdResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public ProductDto? Product { get; set; }

        public static GetProductByIdResult Success(ProductDto product, string message)
        {
            return new GetProductByIdResult
            {
                IsSuccess = true,
                Product = product,
                Message = message
            };
        }

        public static GetProductByIdResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new GetProductByIdResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
