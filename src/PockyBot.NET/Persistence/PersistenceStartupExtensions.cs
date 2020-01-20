using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Persistence
{
    internal static class PersistenceStartupExtensions
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddTransient<IPockyUserRepository, PockyUserRepository>();
            services.AddTransient<IPegRepository, PegRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(databaseConnectionString));
            return services;
        }
    }
}
