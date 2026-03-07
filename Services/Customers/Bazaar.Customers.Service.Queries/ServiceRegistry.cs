using Bazaar.Customers.Service.Queries.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Customers.Service.Queries
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterQueryServices(this IServiceCollection services)
        {
            services.AddScoped<GetCustomerQueryHandler>();

            return services;
        }
    }
}
