using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Responses
{
    public class ChangeBasketCurrencyResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static ChangeBasketCurrencyResult Success(string message)
        {
            return new ChangeBasketCurrencyResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static ChangeBasketCurrencyResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new ChangeBasketCurrencyResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
