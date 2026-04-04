using Bazaar.Products.Service.DataAccess.Context;
using Bazaar.Products.Service.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Products.Service.DataAccess
{
    public static class ServiceRegistry
    {
        public static void RegisterDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ProductsDb")));
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
