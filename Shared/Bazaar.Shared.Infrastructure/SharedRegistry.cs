using Bazaar.Shared.Infrastructure.TimeProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bazaar.Shared.Infrastructure
{
    public static class SharedRegistry
    {
        public static void RegisterSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterDateTimeProvider(services);
        }

        private static void RegisterDateTimeProvider(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }  
    }
}
