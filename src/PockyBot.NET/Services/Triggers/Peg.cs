using System;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Models.Exceptions;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;

namespace PockyBot.NET.Services.Triggers
{
    internal class Peg : ITrigger
    {
        private readonly IPegRequestValidator _pegRequestValidator;
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IPegHelper _pegHelper;
        private readonly IConfigRepository _configRepository;
        private readonly IChatHelper _chatHelper;
        private readonly IPegGiver _pegGiver;
        private readonly ILogger<Peg> _logger;

        public Peg(IPegRequestValidator pegRequestValidator, IPockyUserRepository pockyUserRepository,
            IPegHelper pegHelper, IConfigRepository configRepository, IChatHelper chatHelper,
            IPegGiver pegGiver, ILogger<Peg> logger)
        {
            _pegRequestValidator = pegRequestValidator;
            _pockyUserRepository = pockyUserRepository;
            _pegHelper = pegHelper;
            _configRepository = configRepository;
            _chatHelper = chatHelper;
            _pegGiver = pegGiver;
            _logger = logger;
        }

        public string Command => Commands.Peg;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public string[] Permissions => Array.Empty<string>();

        public async Task<Message> Respond(Message message)
        {
            try
            {
                _logger.LogDebug("Validating peg message");
                _pegRequestValidator.ValidatePegRequest(message);
            }
            catch (PegValidationException e)
            {
                _logger.LogWarning("Peg message is invalid: {@error}", e);
                return new Message
                {
                    Text = e.Message
                };
            }

            _logger.LogDebug("Getting user information for id {userid}, username {username}", message.Sender.UserId,
                message.Sender.Username);
            var sender = await _pockyUserRepository.AddOrUpdateUserAsync(message.Sender.UserId, message.Sender.Username);
            var comment = string.Join(string.Empty, message.MessageParts.Skip(3).Select(x => x.Text)).Trim();

            var requireKeywords = _configRepository.GetGeneralConfig("requireValues");
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword").ToArray();

            var isPegValid = _pegHelper.IsPegValid(comment, requireKeywords, keywords, penaltyKeywords);
            var numPegsGiven = sender.PegsGiven?.Count(x =>
                                   _pegHelper.IsPegValid(x.Comment, requireKeywords, keywords, penaltyKeywords)) ?? 0;

            if (!sender.HasRole(Roles.Unmetered) &&
                (isPegValid && numPegsGiven >= _configRepository.GetGeneralConfig("limit")))
            {
                _logger.LogDebug("User {userId} has reached their peg limit", sender.UserId);
                return new Message
                {
                    Text = "Sorry, but you have already spent all of your pegs for this fortnight."
                };
            }

            var receiverId = message.MessageParts[2].UserId;
            var receiver = await _chatHelper.People.GetPersonAsync(receiverId).ConfigureAwait(false);
            var dbReceiver = await _pockyUserRepository.AddOrUpdateUserAsync(receiver.UserId, receiver.Username);

            _logger.LogInformation(
                "Giving peg with sender {senderId}, receiver {receiverId}, validity {isPegValid}, comment {comment}",
                sender.UserId, receiver.UserId, isPegValid, comment);
            await _pegGiver.GivePeg(comment, sender, dbReceiver, isPegValid ? numPegsGiven + 1 : numPegsGiven).ConfigureAwait(false);
            return null;
        }
    }
}
