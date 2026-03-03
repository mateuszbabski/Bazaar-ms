using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bazaar.Shared.Infrastructure
{
    public static class SharedRegistry
    {
        public static void RegisterSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterServiceAuthExtensions(services, configuration);
        }

        private static IServiceCollection RegisterServiceAuthExtensions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("ServiceJwt")
                .AddJwtBearer("ServiceJwt", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "api-gateway",
                        ValidateAudience = true,
                        ValidAudience = "internal-services",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["ServiceJwt:Secret"]!))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder("ServiceJwt")
                    .RequireAuthenticatedUser()
                    .RequireClaim("service", "api-gateway")
                    .Build();
            });

            return services;
        }
    }
}
