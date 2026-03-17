using Bazaar.Products.Service.Commands.Handlers.AddProduct;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Products.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<AddProductCommandHandler>();

            return services;
        }
    }
}
