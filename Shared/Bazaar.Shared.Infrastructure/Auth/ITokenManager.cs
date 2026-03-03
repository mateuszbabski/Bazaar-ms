namespace Bazaar.Shared.Infrastructure.Auth
{
    public interface ITokenManager
    {
        string GenerateToken(Guid id, string email, string role);
    }
}
