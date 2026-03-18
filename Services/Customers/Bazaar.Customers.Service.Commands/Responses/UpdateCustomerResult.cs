using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Customers.Service.Commands.Responses
{
    public class UpdateCustomerResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static UpdateCustomerResult Success(string message)
        {
            return new UpdateCustomerResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static UpdateCustomerResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new UpdateCustomerResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
