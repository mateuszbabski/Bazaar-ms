using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Commands.Responses
{
    public class AddProductResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static AddProductResult Success(string message)
        {
            return new AddProductResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static AddProductResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new AddProductResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
