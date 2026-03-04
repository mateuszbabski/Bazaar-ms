using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bazaar.Shared.Infrastructure.Auth
{
    public static class RegisterAuthExtensions
    {
        public static IServiceCollection RegisterAuthServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            var serviceTokenSettings = new ServiceTokenSettings();
            configuration.GetSection("ServiceJwt").Bind(serviceTokenSettings);
            services.AddSingleton(serviceTokenSettings);

            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IUserTokenManager>(sp => sp.GetRequiredService<ITokenManager>());
            services.AddSingleton<IServiceTokenManager>(sp => sp.GetRequiredService<ITokenManager>());

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddAuthentication("ServiceJwt")
                .AddJwtBearer("ServiceJwt", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = serviceTokenSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = serviceTokenSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(serviceTokenSettings.Key))
                    };
                });
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder("ServiceJwt")
                    .RequireAuthenticatedUser()
                    .RequireClaim("service",serviceTokenSettings.Issuer)
                    .Build();
            });
            return services;
        }

        public static IServiceCollection RegisterGatewayAuth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            var serviceTokenSettings = new ServiceTokenSettings();
            configuration.GetSection("ServiceJwt").Bind(serviceTokenSettings);
            services.AddSingleton(serviceTokenSettings);

            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IServiceTokenManager>(sp => sp.GetRequiredService<ITokenManager>());

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            return services;
        }
    }
}
