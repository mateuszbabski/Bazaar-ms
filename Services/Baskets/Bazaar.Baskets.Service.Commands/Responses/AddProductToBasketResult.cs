using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Responses
{
    public class AddProductToBasketResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static AddProductToBasketResult Success(string message)
        {
            return new AddProductToBasketResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static AddProductToBasketResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new AddProductToBasketResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
