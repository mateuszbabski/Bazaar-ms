using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Commands.Responses
{
    public class UpdateProductResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static UpdateProductResult Success(string message)
        {
            return new UpdateProductResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static UpdateProductResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new UpdateProductResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
