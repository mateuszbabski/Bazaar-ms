using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Queries.Dtos;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Queries.Responses
{
    public class GetBasketResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public BasketDto? Basket { get; set; }

        public static GetBasketResult Success(BasketDto basket, string message)
        {
            return new GetBasketResult
            {
                IsSuccess = true,
                Basket = basket,
                Message = message
            };
        }

        public static GetBasketResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new GetBasketResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
