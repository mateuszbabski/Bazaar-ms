namespace Bazaar.Shared.Infrastructure.Auth
{
    public class AuthenticationResult
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
