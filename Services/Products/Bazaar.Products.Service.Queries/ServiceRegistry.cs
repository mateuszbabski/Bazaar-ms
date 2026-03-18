using Bazaar.Products.Service.Queries.Handlers.GetAllProducts;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Products.Service.Queries
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterQueryServices(this IServiceCollection services)
        {
            services.AddScoped<GetAllProductsQueryHandler>();

            return services;
        }
    }
}
