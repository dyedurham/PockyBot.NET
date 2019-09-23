using System.Collections.Generic;
using GlobalX.ChatBots.Core;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
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
            var triggerResponseTester = new TriggerResponseTester(wrappedSettings, pockyUserRepository);

            List<ITrigger> triggers = new List<ITrigger>
            {
                new Ping()
            };

            return new PockyBot(triggers, triggerResponseTester);
        }
    }
}
