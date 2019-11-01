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
        public static IPockyBot CreatePockyBot(PockyBotSettings settings, IChatHelper chatHelper)
        {
            var wrappedSettings = new OptionsWrapper<PockyBotSettings>(settings);
            var dbContext = DatabaseContextBuilder.BuildDatabaseContext(settings.DatabaseConnectionString);
            var pockyUserRepository = new PockyUserRepository(dbContext);
            var configRepository = new ConfigRepository(dbContext);
            var pegRepository = new PegRepository(dbContext);
            
            var triggerResponseTester = new TriggerResponseTester(wrappedSettings, pockyUserRepository);
            var pegRequestValidator = new PegRequestValidator(wrappedSettings, configRepository);
            var pegCommentValidator = new PegCommentValidator();
            var pegGiver = new PegGiver(pegRepository, chatHelper);

            List<ITrigger> triggers = new List<ITrigger>
            {
                new Ping(),
                new Peg(pegRequestValidator, pockyUserRepository, pegCommentValidator, configRepository, chatHelper, pegGiver),
                new Status(pockyUserRepository, configRepository, pegCommentValidator),
                new Default(wrappedSettings)
            };

            return new PockyBot(triggers, triggerResponseTester, chatHelper);
        }
    }
}
