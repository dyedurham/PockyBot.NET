using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;

namespace PockyBot.NET.Services.Triggers
{
    internal class Peg : ITrigger
    {
        private readonly IPegRequestValidator _pegRequestValidator;
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IPegCommentValidator _pegCommentValidator;
        private readonly IConfigRepository _configRepository;
        private readonly IChatHelper _chatHelper;
        private readonly IPegGiver _pegGiver;

        public Peg(IPegRequestValidator pegRequestValidator, IPockyUserRepository pockyUserRepository,
            IPegCommentValidator pegCommentValidator, IConfigRepository configRepository, IChatHelper chatHelper,
            IPegGiver pegGiver)
        {
            _pegRequestValidator = pegRequestValidator;
            _pockyUserRepository = pockyUserRepository;
            _pegCommentValidator = pegCommentValidator;
            _configRepository = configRepository;
            _chatHelper = chatHelper;
            _pegGiver = pegGiver;
        }

        public string Command => Commands.Peg;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => true;

        public string[] Permissions => new string[0];

        public async Task<Message> Respond(Message message)
        {
            if (!_pegRequestValidator.ValidatePegRequest(message, out var errorMessage))
            {
                return new Message
                {
                    Text = errorMessage
                };
            }

            var sender = _pockyUserRepository.AddOrUpdateUser(message.SenderId, message.SenderName);
            var comment = string.Join(string.Empty, message.MessageParts.Skip(3).Select(x => x.Text)).Trim();

            var requireKeywords = _configRepository.GetGeneralConfig("requireValues");
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword").ToArray();

            var isPegValid = _pegCommentValidator.IsPegValid(comment, requireKeywords, keywords, penaltyKeywords);
            var numPegsGiven = sender.PegsGiven.Count(x =>
                _pegCommentValidator.IsPegValid(x.Comment, requireKeywords, keywords, penaltyKeywords));

            if (!sender.HasRole(Roles.Unmetered) &&
                 (!isPegValid || numPegsGiven >= _configRepository.GetGeneralConfig("limit")))
            {
                return new Message
                {
                    Text = "Sorry, but you have already spent all of your pegs for this fortnight."
                };
            }

            var receiverId = message.MessageParts[2].UserId;
            var receiver = await _chatHelper.People.GetPersonAsync(receiverId);
            var dbReceiver = _pockyUserRepository.AddOrUpdateUser(receiver.UserId, receiver.Username);

            return await _pegGiver.GivePeg(comment, sender, dbReceiver, isPegValid ? numPegsGiven + 1 : numPegsGiven);
        }
    }
}
