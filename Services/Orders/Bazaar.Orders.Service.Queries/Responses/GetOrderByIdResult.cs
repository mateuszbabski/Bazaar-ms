using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Queries.Dtos;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Queries.Responses
{
    public class GetOrderByIdResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public OrderDto? Order { get; set; }

        public static GetOrderByIdResult Success(OrderDto order, string message)
        {
            return new GetOrderByIdResult
            {
                IsSuccess = true,
                Order = order,
                Message = message
            };
        }

        public static GetOrderByIdResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new GetOrderByIdResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
