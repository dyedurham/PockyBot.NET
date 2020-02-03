using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;

namespace PockyBot.NET.Services.Triggers
{
    class Default : ITrigger
    {
        private readonly PockyBotSettings _settings;

        public string Command => string.Empty;

        public bool DirectMessageAllowed => true;

        public bool CanHaveArgs => true;

        public string[] Permissions => Array.Empty<string>();

        public Default(IOptions<PockyBotSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task<Message> Respond(Message message)
        {
            return Task.FromResult(new Message
            {
                Text = $@"## I'm sorry, I didn't understand that, here's what I can do:

1. To give someone a peg type: `@{_settings.BotName} peg @bob {{comment}}`.
1. For a full list of commands type `@{_settings.BotName} help`.

I am still being worked on, so more features to come : )"
            });
        }
    }
}
