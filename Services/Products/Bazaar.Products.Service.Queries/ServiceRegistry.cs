using Bazaar.Products.Service.Queries.Handlers.GetAllProducts;
using Bazaar.Products.Service.Queries.Handlers.GetProductById;
using Bazaar.Products.Service.Queries.Handlers.GetProductsByShopId;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Products.Service.Queries
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterQueryServices(this IServiceCollection services)
        {
            services.AddScoped<GetAllProductsQueryHandler>();
            services.AddScoped<GetProductByIdQueryHandler>();
            services.AddScoped<GetProductsByShopIdQueryHandler>();

            return services;
        }
    }
}
