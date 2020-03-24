using System.Collections.Generic;
using GlobalX.ChatBots.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET
{
    public static class PockyBotFactory
    {
        public static IPockyBot CreatePockyBot(PockyBotSettings settings, IChatHelper chatHelper, IResultsUploader resultsUploader, ILoggerFactory loggerFactory)
        {
            var wrappedSettings = new OptionsWrapper<PockyBotSettings>(settings);
            var dbContext = DatabaseContextBuilder.BuildDatabaseContext(settings.DatabaseConnectionString);
            var pockyUserRepository = new PockyUserRepository(dbContext);
            var configRepository = new ConfigRepository(dbContext);
            var pegRepository = new PegRepository(dbContext);
            var locationRepository = new LocationRepository(dbContext);
            var userLocationRepository = new UserLocationRepository(dbContext);

            var triggerResponseTester = new TriggerResponseTester(wrappedSettings, pockyUserRepository);
            var pegRequestValidator = new PegRequestValidator(wrappedSettings, configRepository);
            var pegHelper = new PegHelper(configRepository);
            var pegGiver = new PegGiver(pegRepository, chatHelper);
            var directResultsMessageSender = new DirectResultsMessageSender(chatHelper.Messages);
            var pegResultsHelper = new PegResultsHelper(configRepository, pegHelper);
            var userLocationGetter = new UserLocationGetter(pockyUserRepository);
            var userLocationSetter =
                new UserLocationSetter(pockyUserRepository, locationRepository, userLocationRepository, chatHelper);
            var userLocationDeleter = new UserLocationDeleter(userLocationRepository);

            var triggers = new List<ITrigger>
            {
                new Ping(),
                new Help(pockyUserRepository, wrappedSettings, configRepository),
                new Welcome(wrappedSettings, configRepository),
                new Peg(pegRequestValidator, pockyUserRepository, pegHelper, configRepository, chatHelper, pegGiver, loggerFactory.CreateLogger<Peg>()),
                new Status(pockyUserRepository, configRepository, pegHelper, loggerFactory.CreateLogger<Status>()),
                new Finish(pockyUserRepository, pegResultsHelper, resultsUploader, directResultsMessageSender, loggerFactory.CreateLogger<Finish>()),
                new Reset(pegRepository, loggerFactory.CreateLogger<Reset>()),
                new Keywords(configRepository),
                new Rotation(configRepository),
                new LocationConfig(locationRepository, pockyUserRepository),
                new UserLocation(pockyUserRepository, userLocationGetter, userLocationSetter, userLocationDeleter),
                new StringConfig(configRepository),
                new RoleConfig(pockyUserRepository, chatHelper),
                new RemoveUser(pockyUserRepository, loggerFactory.CreateLogger<RemoveUser>()),
                new LocationWeight(configRepository, locationRepository),
                new Default(wrappedSettings)
            };

            return new PockyBot(triggers, triggerResponseTester, chatHelper, loggerFactory.CreateLogger<PockyBot>());
        }
    }
}
