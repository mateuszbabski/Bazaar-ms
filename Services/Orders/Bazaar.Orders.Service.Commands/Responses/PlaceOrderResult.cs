using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Commands.Responses
{
    public class PlaceOrderResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public Guid? OrderId { get; set; }

        public static PlaceOrderResult Success(string message, Guid orderId)
        {
            return new PlaceOrderResult
            {
                IsSuccess = true,
                Message = message,
                OrderId = orderId
            };
        }

        public static PlaceOrderResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new PlaceOrderResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
