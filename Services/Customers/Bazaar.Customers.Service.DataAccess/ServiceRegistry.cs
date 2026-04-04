using Bazaar.Customers.Service.DataAccess.Context;
using Bazaar.Customers.Service.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Customers.Service.DataAccess
{
    public static class ServiceRegistry
    {
        public static void RegisterDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomersDbContext>(options =>            
                options.UseSqlServer(configuration.GetConnectionString("CustomersDb")));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
