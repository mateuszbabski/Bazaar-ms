using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Customers.Service.Queries.Dtos;

namespace Bazaar.Customers.Service.Queries.Responses
{
    public class GetCustomerResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCode? ErrorCode { get; set; }
        public CustomerDto? Customer { get; set; }

        public static GetCustomerResult Success(CustomerDto customer, string message)
        {
            return new GetCustomerResult
            {
                IsSuccess = true,
                Customer = customer,
                Message = message
            };
        }

        public static GetCustomerResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new GetCustomerResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
