using Bazaar.Shared.Infrastructure.TimeProvider;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bazaar.Shared.Infrastructure.Auth
{
    internal class TokenManager : ITokenManager
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;
        private readonly ServiceTokenSettings _serviceTokenSettings;

        public TokenManager(
            IDateTimeProvider dateTimeProvider, 
            JwtSettings jwtSettings, 
            ServiceTokenSettings serviceTokenSettings)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtSettings;
            _serviceTokenSettings = serviceTokenSettings;
        }

        public string GenerateServiceToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_serviceTokenSettings.Key));

            var claims = new[] {
                new Claim("sub", _serviceTokenSettings.Issuer),
                new Claim("service", _serviceTokenSettings.Issuer)
            };

            var token = new JwtSecurityToken(
                issuer: _serviceTokenSettings.Issuer,
                audience: _serviceTokenSettings.Audience,
                claims: claims,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_serviceTokenSettings.DurationInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken(Guid id, string email, string role)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
