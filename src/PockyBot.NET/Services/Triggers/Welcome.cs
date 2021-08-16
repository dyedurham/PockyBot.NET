using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Welcome : ITrigger
    {
        private readonly IConfigRepository _configRepository;
        private readonly PockyBotSettings _pockyBotSettings;

        public string Command => Commands.Welcome;
        public bool DirectMessageAllowed => true;
        public bool CanHaveArgs => true;
        public Role[] Permissions => Array.Empty<Role>();

        public Welcome(IOptions<PockyBotSettings> pockyBotOptions, IConfigRepository configRepository)
        {
            _configRepository = configRepository;
            _pockyBotSettings = pockyBotOptions.Value;
        }

        public Task<Message> Respond(Message message)
        {
            var newMessage = "## Hello world!\n" +
                             $"I'm {_pockyBotSettings.BotName}. I help you spread the word about the great work that your team mates are doing! " +
                             "I hand out pegs to everyone you tell me about. ";

            var keywordsRequired = _configRepository.GetGeneralConfig("requireValues") == 1;
            if (keywordsRequired)
            {
                var keywords = string.Join(", ", _configRepository.GetStringConfig("keyword"));
                newMessage +=  $"Tell us why you're giving them a peg and include the relevant company values in your description: {keywords}.";
            }
            else
            {
                newMessage += "Make sure to tell us why you're giving them a peg.";
            }
            newMessage += "\n\nBut also... if you spot someone shaming our PC security by leaving their desktop unlocked - you can award them a shame peg!\n\n" +
                          $"Find out how I work by typing @{_pockyBotSettings.BotName} help";

            return Task.FromResult(new Message
            {
                Text = newMessage
            });
        }
    }
}
