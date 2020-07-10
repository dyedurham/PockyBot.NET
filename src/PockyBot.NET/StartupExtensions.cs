using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Services.UserLocation;

namespace PockyBot.NET
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigurePockyBot(this IServiceCollection services, PockyBotSettings settings) {
            var options = new OptionsWrapper<PockyBotSettings>(settings);
            services.AddSingleton<IOptions<PockyBotSettings>>(options);
            services.ConfigureCommonServices();
            services.ConfigurePersistenceServices(settings.DatabaseConnectionString);
            return services;
        }

        public static IServiceCollection ConfigurePockyBot(this IServiceCollection services, IConfiguration configuration) {
            var section = configuration.GetSection("PockyBot.NET");
            services.Configure<PockyBotSettings>(section);
            services.ConfigureCommonServices();
            services.ConfigurePersistenceServices(section["DatabaseConnectionString"]);
            return services;
        }

        private static void ConfigureCommonServices(this IServiceCollection services) {
            services.AddTransient<ITriggerResponseTester, TriggerResponseTester>();
            services.AddTransient<IPegRequestValidator, PegRequestValidator>();
            services.AddTransient<IPegHelper, PegHelper>();
            services.AddTransient<IPegGiver, PegGiver>();
            services.AddTransient<IPockyBot, PockyBot>();
            services.AddTransient<IPegResultsHelper, PegResultsHelper>();
            services.AddTransient<IDirectResultsMessageSender, DirectResultsMessageSender>();
            services.AddTransient<IUserLocationGetter, UserLocationGetter>();
            services.AddTransient<IUserLocationSetter, UserLocationSetter>();
            services.AddTransient<IUserLocationDeleter, UserLocationDeleter>();
            services.AddTransient<IResultsFileGenerator, ResultsFileGenerator>();
            services.AddTransient<IUsernameUpdater, UsernameUpdater>();
            services.AddSingleton<IRandomnessHandler, RandomnessHandler>();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddTransient<ITrigger, Ping>();
            services.AddTransient<ITrigger, Help>();
            services.AddTransient<ITrigger, Welcome>();
            services.AddTransient<ITrigger, Peg>();
            services.AddTransient<ITrigger, Unpeg>();
            services.AddTransient<ITrigger, Status>();
            services.AddTransient<ITrigger, Results>();
            services.AddTransient<ITrigger, Finish>();
            services.AddTransient<ITrigger, Reset>();
            services.AddTransient<ITrigger, Keywords>();
            services.AddTransient<ITrigger, Rotation>();
            services.AddTransient<ITrigger, LocationConfig>();
            services.AddTransient<ITrigger, UserLocation>();
            services.AddTransient<ITrigger, NumberConfig>();
            services.AddTransient<ITrigger, StringConfig>();
            services.AddTransient<ITrigger, RoleConfig>();
            services.AddTransient<ITrigger, RemoveUser>();
            services.AddTransient<ITrigger, LocationWeight>();
            services.AddTransient<ITrigger, Default>();
        }
    }
}
