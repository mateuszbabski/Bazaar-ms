using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Commands.Responses
{
    public class CancelOrderResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static CancelOrderResult Success(string message)
        {
            return new CancelOrderResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static CancelOrderResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new CancelOrderResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
