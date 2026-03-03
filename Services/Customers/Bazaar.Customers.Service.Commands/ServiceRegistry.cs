using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Customers.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<SignUpCommandHandler>();

            return services;
        }
    }
}
