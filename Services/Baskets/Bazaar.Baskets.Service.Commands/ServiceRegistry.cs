using Bazaar.Baskets.Service.Commands.Clients;
using Bazaar.Baskets.Service.Commands.Handlers.AddProductToBasket;
using Bazaar.Baskets.Service.Commands.Handlers.ChangeBasketCurrency;
using Bazaar.Baskets.Service.Commands.Handlers.ChangeItemQuantity;
using Bazaar.Baskets.Service.Commands.Handlers.CheckoutBasket;
using Bazaar.Baskets.Service.Commands.Handlers.ClearBasket;
using Bazaar.Baskets.Service.Commands.Handlers.RemoveProductFromBasket;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Baskets.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<AddProductToBasketCommandHandler>();
            services.AddScoped<RemoveProductFromBasketCommandHandler>();
            services.AddScoped<ChangeBasketCurrencyCommandHandler>();
            services.AddScoped<ChangeItemQuantityCommandHandler>();
            services.AddScoped<CheckoutBasketCommandHandler>();
            services.AddScoped<ClearBasketCommandHandler>();

            services.AddTransient<ProductServiceClient>();
            services.AddTransient<CurrencyRateClient>();
            services.AddTransient<OrderServiceClient>();

            return services;
        }
    }
}
