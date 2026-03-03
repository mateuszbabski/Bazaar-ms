using Bazaar.Customers.Service.Commands.Handlers.SignIn;
using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Customers.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<SignUpCommandHandler>();
            services.AddScoped<SignInCommandHandler>();

            return services;
        }
    }
}
