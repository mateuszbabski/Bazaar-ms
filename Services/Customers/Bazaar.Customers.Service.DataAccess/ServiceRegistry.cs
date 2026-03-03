using Bazaar.Customers.Service.DataAccess.Context;
using Bazaar.Customers.Service.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Customers.Service.DataAccess
{
    public static class ServiceRegistry
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomersDbContext>(options =>            
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
