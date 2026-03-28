using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Responses
{
    public class RemoveProductFromBasketResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static RemoveProductFromBasketResult Success(string message)
        {
            return new RemoveProductFromBasketResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static RemoveProductFromBasketResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new RemoveProductFromBasketResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
