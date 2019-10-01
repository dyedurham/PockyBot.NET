using System;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Pegs
{
    internal class PegRequestValidator : IPegRequestValidator
    {
        private readonly PockyBotSettings _settings;
        private readonly IConfigRepository _configRepository;

        public PegRequestValidator(IOptions<PockyBotSettings> settings, IConfigRepository configRepository)
        {
            _settings = settings.Value;
            _configRepository = configRepository;
        }

        public bool ValidatePegRequest(Message message, out string errorMessage)
        {
            if (message.MessageParts.Length < 3
                || message.MessageParts[1].MessageType != MessageType.Text
                || !string.Equals(message.MessageParts[1].Text.Trim(), Commands.Peg, StringComparison.InvariantCultureIgnoreCase)
                || message.MessageParts[2].MessageType != MessageType.PersonMention)
            {
                errorMessage = $"I'm sorry, I couldn't understand your peg request. Please use the following format: `@{_settings.BotName} peg @Person this is the reason for giving you a peg`.";
                return false;
            }

            var comment = string.Join(string.Empty, message.MessageParts.Skip(3).Select(x => x.Text)).Trim();
            if (_configRepository.GetGeneralConfig("commentsRequired") == 1 && string.IsNullOrWhiteSpace(comment))
            {
                errorMessage = "Please provide a comment with your peg.";
                return false;
            }

            var keywords = _configRepository.GetStringConfig("keyword");
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword");
            if (_configRepository.GetGeneralConfig("requireValues") == 1
                && (!keywords.Any(x => comment.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0)
                && !penaltyKeywords.Any(x => comment.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0)))
            {
                errorMessage = $"I'm sorry, you have to include a keyword in your comment. Please include one of the below keywords in your comment:\n\n{string.Join(", ", keywords)}";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
