namespace Bazaar.Shared.Infrastructure.Auth
{
    public class AuthenticationResult
    {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static AuthenticationResult Success(Guid id, string token)
        {
            return new AuthenticationResult
            {
                Id = id,
                Token = token,
                IsSuccess = true,
                Message = "Authentication successful."
            };
        }

        public static AuthenticationResult Failure(string message)
        {
            return new AuthenticationResult
            {
                Id = Guid.Empty,
                Token = string.Empty,
                IsSuccess = false,
                Message = message
            };
        }
    }
}
