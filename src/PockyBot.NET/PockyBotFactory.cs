using System.Collections.Generic;
using GlobalX.ChatBots.Core;
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
        public static IPockyBot CreatePockyBot(PockyBotSettings settings, IChatHelper chatHelper, IResultsUploader resultsUploader)
        {
            var wrappedSettings = new OptionsWrapper<PockyBotSettings>(settings);
            var dbContext = DatabaseContextBuilder.BuildDatabaseContext(settings.DatabaseConnectionString);
            var pockyUserRepository = new PockyUserRepository(dbContext);
            var configRepository = new ConfigRepository(dbContext);
            var pegRepository = new PegRepository(dbContext);

            var triggerResponseTester = new TriggerResponseTester(wrappedSettings, pockyUserRepository);
            var pegRequestValidator = new PegRequestValidator(wrappedSettings, configRepository);
            var pegHelper = new PegHelper(configRepository);
            var pegGiver = new PegGiver(pegRepository, chatHelper);
            var directResultsMessageSender = new DirectResultsMessageSender(chatHelper.Messages);
            var pegResultsHelper = new PegResultsHelper(configRepository, pegHelper);

            List<ITrigger> triggers = new List<ITrigger>
            {
                new Ping(),
                new Help(),
                new Peg(pegRequestValidator, pockyUserRepository, pegHelper, configRepository, chatHelper, pegGiver),
                new Status(pockyUserRepository, configRepository, pegHelper),
                new Finish(pockyUserRepository, pegResultsHelper, resultsUploader, directResultsMessageSender),
                new Reset(pegRepository),
                new Default(wrappedSettings)
            };

            return new PockyBot(triggers, triggerResponseTester, chatHelper);
        }
    }
}
