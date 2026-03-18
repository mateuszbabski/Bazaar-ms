using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Commands.Responses
{
    public class DeleteProductResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static DeleteProductResult Success(string message)
        {
            return new DeleteProductResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static DeleteProductResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new DeleteProductResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
