using Bazaar.Baskets.Service.DataAccess.Context;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Baskets.Service.DataAccess
{
    public static class ServiceRegistry
    {
        public static void RegisterDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BasketsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BasketsDb")));
            services.AddScoped<IBasketRepository, BasketRepository>();
        }
    }
}
