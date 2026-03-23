using Bazaar.Baskets.Service.Commands.Clients;
using Bazaar.Baskets.Service.Commands.Handlers.AddProductToBasket;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Baskets.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<AddProductToBasketCommandHandler>();
            services.AddTransient<ProductServiceClient>();

            return services;
        }
    }
}
