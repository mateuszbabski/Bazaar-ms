using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Commands.Responses
{
    public class PayForOrderResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static PayForOrderResult Success(string message)
        {
            return new PayForOrderResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static PayForOrderResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new PayForOrderResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
