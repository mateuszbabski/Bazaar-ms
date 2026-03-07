using Bazaar.Customers.Service.Domain.ErrorCodes;

namespace Bazaar.Customers.Service.Commands.Responses
{
    public class SignUpResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
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
                Error = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
