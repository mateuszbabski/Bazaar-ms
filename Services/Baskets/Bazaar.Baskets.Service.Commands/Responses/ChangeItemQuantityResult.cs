using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Responses
{
    public class ChangeItemQuantityResult : Result
    {
        public ErrorCode? ErrorCode { get; set; }

        public static ChangeItemQuantityResult Success(string message)
        {
            return new ChangeItemQuantityResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static ChangeItemQuantityResult Failure(string errorMessage, ErrorCode errorCode)
        {
            return new ChangeItemQuantityResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
    }
}
