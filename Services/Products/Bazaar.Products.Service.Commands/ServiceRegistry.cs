using Bazaar.Products.Service.Commands.Handlers.AddProduct;
using Bazaar.Products.Service.Commands.Handlers.DeleteProduct;
using Bazaar.Products.Service.Commands.Handlers.UpdateProduct;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Products.Service.Commands
{
    public static class ServiceRegistry
    {
        public static IServiceCollection RegisterCommandServices(this IServiceCollection services)
        {
            services.AddScoped<AddProductCommandHandler>();
            services.AddScoped<UpdateProductCommandHandler>();
            services.AddScoped<DeleteProductCommandHandler>();

            return services;
        }
    }
}
