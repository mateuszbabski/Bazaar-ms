using Bazaar.Products.Service.Domain.ErrorCodes;

namespace Bazaar.Products.Service.Commands.Responses
{
    public class AddProductCommandResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCode? ErrorCode { get; set; }

        public static AddProductCommandResult Success(string message)
        {
            return new AddProductCommandResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static AddProductCommandResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new AddProductCommandResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
