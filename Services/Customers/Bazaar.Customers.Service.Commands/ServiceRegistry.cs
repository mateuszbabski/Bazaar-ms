using Bazaar.Customers.Service.Commands.Handlers.SignIn;
using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Bazaar.Customers.Service.Commands.Handlers.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Customers.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<SignUpCommandHandler>();
            services.AddScoped<SignInCommandHandler>();
            services.AddScoped<UpdateCustomerCommandHandler>();

            return services;
        }
    }
}
