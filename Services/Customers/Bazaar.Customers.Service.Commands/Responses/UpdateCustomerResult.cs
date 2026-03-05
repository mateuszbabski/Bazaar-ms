namespace Bazaar.Customers.Service.Commands.Responses
{
    public class UpdateCustomerResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; } 
        public ErrorCode? ErrorCode { get; set; }

        public static UpdateCustomerResult Success(string message)
        {
            return new UpdateCustomerResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static UpdateCustomerResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new UpdateCustomerResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
