using Bazaar.Baskets.Service.Queries.Handlers.GetBasket;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Baskets.Service.Queries
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterQueryServices(this IServiceCollection services)
        {
            services.AddScoped<GetBasketQueryHandler>();

            return services;
        }
    }
}
