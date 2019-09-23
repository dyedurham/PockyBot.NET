using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Persistence
{
    public static class Extensions
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddTransient<IPockyUserRepository, PockyUserRepository>();
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(databaseConnectionString));
            return services;
        }
    }
}
