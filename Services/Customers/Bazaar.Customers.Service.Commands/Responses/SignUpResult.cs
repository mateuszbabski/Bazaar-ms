using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Customers.Service.Commands.Responses
{
    public class SignUpResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static SignUpResult Success(string message)
        {
            return new SignUpResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static SignUpResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new SignUpResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
