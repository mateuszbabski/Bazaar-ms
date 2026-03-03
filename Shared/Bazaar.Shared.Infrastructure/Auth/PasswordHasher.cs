namespace Bazaar.Shared.Infrastructure.Auth
{
    internal class PasswordHasher : IPasswordHasher
    {
        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(8);
        }

        public string GenerateHashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GenerateSalt());
        }

        public bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
