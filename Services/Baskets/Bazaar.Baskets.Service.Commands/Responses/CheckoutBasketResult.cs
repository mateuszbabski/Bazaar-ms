using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Responses
{
    public class CheckoutBasketResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static CheckoutBasketResult Success(string message)
        {
            return new CheckoutBasketResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static CheckoutBasketResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new CheckoutBasketResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
