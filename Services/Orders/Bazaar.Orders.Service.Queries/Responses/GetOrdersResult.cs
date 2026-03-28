using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Queries.Dtos;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Queries.Responses
{
    public class GetOrdersResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }
        public List<OrderDto>? Orders { get; set; }

        public static GetOrdersResult Success(List<OrderDto> orders, string message)
        {
            return new GetOrdersResult
            {
                IsSuccess = true,
                Orders = orders,
                Message = message
            };
        }

        public static GetOrdersResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new GetOrdersResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
