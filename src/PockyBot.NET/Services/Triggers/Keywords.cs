using System;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Keywords : ITrigger
    {
        private readonly IConfigRepository _configRepository;

        public string Command => Commands.Keywords;

        public bool DirectMessageAllowed => true;

        public bool CanHaveArgs => false;

        public Role[] Permissions => Array.Empty<Role>();

        public Keywords(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public Task<Message> Respond(Message message)
        {
            var messageBuilder = new StringBuilder();
            var keywords = _configRepository.GetStringConfig("keyword");
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword");

            if (keywords.Count == 0)
            {
                messageBuilder.Append("No keywords set.\n\n");
            }
            else
            {
                messageBuilder.Append("## Here is the list of possible keywords to include in your message\n\n* ");
                messageBuilder.Append(string.Join("\n* ", keywords));
                messageBuilder.Append("\n\n");
            }

            if (penaltyKeywords.Count == 0)
            {
                messageBuilder.Append("No penalty keywords set.");
            }
            else
            {
                messageBuilder.Append(
                    "## Here is the list of keywords that can be used to apply a penalty to the sender\n\n");
                messageBuilder.Append(
                    "Penalty keywords do not count against the peg limit, and are *not* applied to messages that also include standard keywords.\n\n* ");
                messageBuilder.Append(string.Join("\n* ", penaltyKeywords));
            }

            return Task.FromResult(new Message
            {
                Text = messageBuilder.ToString()
            });
        }
    }
}
