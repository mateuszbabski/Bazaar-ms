using Bazaar.Orders.Service.Commands.Handlers.CancelOrder;
using Bazaar.Orders.Service.Commands.Handlers.PayForOrder;
using Bazaar.Orders.Service.Commands.Handlers.PlaceOrder;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Orders.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<PlaceOrderCommandHandler>();
            services.AddScoped<PayForOrderCommandHandler>();
            services.AddScoped<CancelOrderCommandHandler>();

            return services;
        }
    }
}
