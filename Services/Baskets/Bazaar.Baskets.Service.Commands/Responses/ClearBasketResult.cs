using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Responses
{
    public class ClearBasketResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static ClearBasketResult Success(string message)
        {
            return new ClearBasketResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static ClearBasketResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new ClearBasketResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
