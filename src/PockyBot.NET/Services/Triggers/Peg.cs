using System;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Models;
using PockyBot.NET.Models.Exceptions;
using PockyBot.NET.Persistence.Models;
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

        public Role[] Permissions => Array.Empty<Role>();

        public async Task<Message> Respond(Message message)
        {
            if (!IsPegFormatValid(message, out var pegFormatErrorMessage))
            {
                return pegFormatErrorMessage;
            }

            _logger.LogDebug("Getting user information for id {userid}, username {username}", message.Sender.UserId,
                message.Sender.Username);
            var sender = await _pockyUserRepository.AddOrUpdateUserAsync(message.Sender.UserId, message.Sender.Username);

            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword").ToArray();
            var keywords = GetKeywords();

            var comment = string.Join(string.Empty, message.MessageParts.Skip(3).Select(x => x.Text)).Trim();
            var isPenaltyPeg = _pegHelper.IsPenaltyPeg(comment, penaltyKeywords, keywords);
            var numPegsGiven = sender.PegsGiven?
                .Count(x => !_pegHelper.IsPenaltyPeg(x.Comment, penaltyKeywords, keywords)) ?? 0;


            if (!UserHasPegsRemaining(sender, isPenaltyPeg, numPegsGiven, out var pegsRemainingErrorMessage))
            {
                return pegsRemainingErrorMessage;
            }

            if (!IsPegMessageValid(message, out var pegMessageErrorMessage))
            {
                return pegMessageErrorMessage;
            }

            await GivePeg(message, comment, sender, isPenaltyPeg, numPegsGiven);
            return null;
        }

        public string GetHelpMessage(string botName, PockyUser user)
        {
            var keywordsRequired = _configRepository.GetGeneralConfig("requireValues") == 1;
            var newMessage = "### How to give a peg 🎁!\n" +
                             $"1. To give someone a peg type: `@{botName} {Commands.Peg} @bob {{comment}}`.\n";

            if (keywordsRequired) {
                newMessage += "1. Note that your comment MUST include a keyword.";
            }
            return newMessage;
        }

        private bool IsPegFormatValid(Message message, out Message errorMessage)
        {
            try
            {
                _logger.LogDebug("Validating peg request format");
                _pegRequestValidator.ValidatePegRequestFormat(message);
            }
            catch (PegValidationException e)
            {
                _logger.LogWarning("Peg message format is invalid: {@error}", e);
                errorMessage = new Message
                {
                    Text = e.Message
                };
                return false;
            }
            errorMessage = null;
            return true;
        }

        private bool UserHasPegsRemaining(PockyUser sender, bool isPenaltyPeg, int numPegsGiven, out Message errorMessage)
        {
            if (!sender.HasRole(Role.Unmetered) && !isPenaltyPeg &&
                (numPegsGiven >= _configRepository.GetGeneralConfig("limit")))
            {
                _logger.LogDebug("User {userId} has reached their peg limit", sender.UserId);
                errorMessage = new Message
                {
                    Text = "Sorry, but you have already spent all of your pegs for this fortnight."
                };
                return false;
            }
            errorMessage = null;
            return true;
        }

        private bool IsPegMessageValid(Message message, out Message errorMessage)
        {
            try
            {
                _logger.LogDebug("Validating peg message");
                _pegRequestValidator.ValidatePegRequest(message);
            }
            catch (PegValidationException e)
            {
                _logger.LogWarning("Peg message is invalid: {@error}", e);
                errorMessage = new Message
                {
                    Text = e.Message
                };
                return false;
            }
            errorMessage = null;
            return true;
        }

        private string[] GetKeywords()
        {
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            var linkedKeywords = _configRepository.GetStringConfig("linkedKeyword").Select(x => new LinkedKeyword(x));
            return keywords.Concat(linkedKeywords.Select(x => x.LinkedWord)).ToArray();
        }

        private async Task GivePeg(Message message, string comment, PockyUser sender, bool isPenaltyPeg, int numPegsGiven)
        {
            var receiverId = message.MessageParts[2].UserId;
            var receiver = await _chatHelper.People.GetPersonAsync(receiverId).ConfigureAwait(false);
            var dbReceiver = await _pockyUserRepository.AddOrUpdateUserAsync(receiver.UserId, receiver.Username);

            _logger.LogInformation(
                "Giving peg with sender {senderId}, receiver {receiverId}, is penalty peg? {isPegValid}, comment {comment}",
                sender.UserId, receiver.UserId, isPenaltyPeg, comment);
            await _pegGiver.GivePeg(comment, sender, dbReceiver, !isPenaltyPeg ? numPegsGiven + 1 : numPegsGiven).ConfigureAwait(false);
        }
    }
}
