namespace Bazaar.Shared.Infrastructure.Auth
{
    public interface IPasswordHasher
    {
        string GenerateSalt();
        string GenerateHashPassword(string password);
        bool ValidatePassword(string password, string correctHash);
    }
}
