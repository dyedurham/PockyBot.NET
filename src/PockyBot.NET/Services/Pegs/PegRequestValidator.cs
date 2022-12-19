using System;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using PockyBot.NET.Configuration;
using PockyBot.NET.Constants;
using PockyBot.NET.Models;
using PockyBot.NET.Models.Exceptions;
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

        public void ValidatePegRequest(Message message)
        {
            ValidateRecipient(message);
            var comment = GetCommentText(message);
            ValidateComment(comment);

            ValidateKeywords(comment);
        }

        public void ValidatePegRequestFormat(Message message)
        {
            ValidateMessageFormat(message);
        }

        private void ValidateMessageFormat(Message message)
        {
            if (message.MessageParts.Length < 3
                || message.MessageParts[1].MessageType != MessageType.Text
                || !string.Equals(message.MessageParts[1].Text.Trim(), Commands.Peg, StringComparison.OrdinalIgnoreCase)
                || message.MessageParts[2].MessageType != MessageType.PersonMention)
            {
                throw new PegValidationException(
                    $"I'm sorry, I couldn't understand your peg request. Please use the following format: `@{_settings.BotName} peg @Person this is the reason for giving you a peg`.");
            }
        }

        private void ValidateRecipient(Message message)
        {
            if (message.Sender.UserId == message.MessageParts[2].UserId)
            {
                throw new PegValidationException(
                    $"You can't peg yourself.");
            }
            if (message.MessageParts[2].UserId == _settings.BotId){
                throw new PegValidationException("You can't give pegs to me.");
            }
        }

        private static string GetCommentText(Message message)
        {
            return string.Join(string.Empty, message.MessageParts.Skip(3).Select(x => x.Text)).Trim();
        }

        private void ValidateComment(string comment)
        {
            if (_configRepository.GetGeneralConfig("commentsRequired") == 1 && string.IsNullOrWhiteSpace(comment))
            {
                throw new PegValidationException("Please provide a comment with your peg.");
            }
        }

        private void ValidateKeywords(string comment)
        {
            var keywords = _configRepository.GetStringConfig("keyword");
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword");
            var linkedKeywords = _configRepository.GetStringConfig("linkedKeyword").Select(x => new LinkedKeyword(x));
            var validLinkedWords =
                linkedKeywords.Where(x => keywords.Contains(x.Keyword, StringComparer.OrdinalIgnoreCase)).Select(x => x.LinkedWord);

            if (_configRepository.GetGeneralConfig("requireValues") == 1
                && !keywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0)
                    && !penaltyKeywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0)
                        && !validLinkedWords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                throw new PegValidationException(
                    $"I'm sorry, you have to include a keyword in your comment. Please include one of the below keywords in your comment:\n\n{string.Join(", ", keywords)}");
            }
        }
    }
}
