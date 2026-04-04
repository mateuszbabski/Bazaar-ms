using Bazaar.Orders.Service.DataAccess.Context;
using Bazaar.Orders.Service.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Orders.Service.DataAccess
{
    public static class ServiceRegistry
    {
        public static void RegisterDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrdersDb")));
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
