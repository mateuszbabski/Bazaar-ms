using Bazaar.Orders.Service.Queries.Handlers.GetOrderById;
using Bazaar.Orders.Service.Queries.Handlers.GetOrdersByCustomerId;
using Bazaar.Orders.Service.Queries.Handlers.GetOrdersByShopId;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Orders.Service.Queries
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterQueryServices(this IServiceCollection services)
        {
            services.AddScoped<GetOrdersByCustomerIdQueryHandler>();
            services.AddScoped<GetOrderByIdQueryHandler>();
            services.AddScoped<GetOrdersByShopIdQueryHandler>();

            return services;
        }
    }
}
