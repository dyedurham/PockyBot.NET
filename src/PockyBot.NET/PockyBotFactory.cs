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
using PockyBot.NET.Services.UserLocation;

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
            var directResultsMessageSender = new DirectResultsMessageSender(chatHelper.Messages, loggerFactory.CreateLogger<DirectResultsMessageSender>());
            var pegResultsHelper = new PegResultsHelper(configRepository, pegHelper);
            var userLocationGetter = new UserLocationGetter(pockyUserRepository);
            var userLocationSetter =
                new UserLocationSetter(pockyUserRepository, locationRepository, userLocationRepository, chatHelper);
            var userLocationDeleter = new UserLocationDeleter(userLocationRepository);
            var resultsFileGenerator = new ResultsFileGenerator(pegResultsHelper, loggerFactory.CreateLogger<ResultsFileGenerator>(), resultsUploader);
            var usernameUpdater = new UsernameUpdater(chatHelper.People, pockyUserRepository, loggerFactory.CreateLogger<UsernameUpdater>());

            var ping = new Ping();
            var welcome = new Welcome(wrappedSettings, configRepository);
            var peg = new Peg(pegRequestValidator, pockyUserRepository, pegHelper, configRepository, chatHelper, pegGiver, loggerFactory.CreateLogger<Peg>());
            var status = new Status(pockyUserRepository, configRepository, pegHelper, loggerFactory.CreateLogger<Status>());
            var results = new Results(pockyUserRepository, pegResultsHelper, resultsFileGenerator, loggerFactory.CreateLogger<Results>());
            var finish = new Finish(pockyUserRepository, pegResultsHelper, directResultsMessageSender, resultsFileGenerator, loggerFactory.CreateLogger<Finish>(), usernameUpdater);
            var reset = new Reset(pegRepository, loggerFactory.CreateLogger<Reset>());
            var keywords = new Keywords(configRepository);
            var rotation = new Rotation(configRepository);
            var locationConfig = new LocationConfig(locationRepository, pockyUserRepository);
            var userLocation = new UserLocation(pockyUserRepository, userLocationGetter, userLocationSetter, userLocationDeleter);
            var numberConfig = new NumberConfig(configRepository);
            var stringConfig = new StringConfig(configRepository);
            var roleConfig = new RoleConfig(pockyUserRepository, chatHelper);
            var removeUser = new RemoveUser(pockyUserRepository, loggerFactory.CreateLogger<RemoveUser>());
            var locationWeight = new LocationWeight(configRepository, locationRepository);
            var triggers = new List<ITrigger>
            {
                ping,
                new Help(pockyUserRepository, wrappedSettings,
                    new IHelpMessageTrigger[]
                    {
                        ping, welcome, peg, status, results, finish, reset, keywords, rotation, locationConfig,
                        userLocation, numberConfig, stringConfig, roleConfig, removeUser, locationWeight
                    }),
                welcome,
                peg,
                status,
                results,
                finish,
                reset,
                keywords,
                rotation,
                locationConfig,
                userLocation,
                numberConfig,
                stringConfig,
                roleConfig,
                removeUser,
                locationWeight,
                new Default(wrappedSettings)
            };

            return new PockyBot(triggers, triggerResponseTester, chatHelper, loggerFactory.CreateLogger<PockyBot>());
        }
    }
}
