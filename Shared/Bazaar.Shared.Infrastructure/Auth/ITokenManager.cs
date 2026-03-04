namespace Bazaar.Shared.Infrastructure.Auth
{
    public interface IUserTokenManager
    {
        string GenerateToken(Guid id, string email, string role);        
    }

    public interface IServiceTokenManager
    {
        string GenerateServiceToken();
    }
     public interface ITokenManager : IUserTokenManager, IServiceTokenManager
    {
    }
}
