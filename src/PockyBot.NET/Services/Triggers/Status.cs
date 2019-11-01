using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;

namespace PockyBot.NET.Services.Triggers
{
    class Status : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IPegCommentValidator _pegCommentValidator;

        public Status(IPockyUserRepository pockyUserRepository, IConfigRepository configRepository, IPegCommentValidator pegCommentValidator)
        {
            _pockyUserRepository = pockyUserRepository;
            _configRepository = configRepository;
            _pegCommentValidator = pegCommentValidator;
        }

        public string Command => Commands.Status;

        public bool DirectMessageAllowed => true;

        public bool CanHaveArgs => false;

        public string[] Permissions => Array.Empty<string>();

        public Task<Message> Respond(Message message)
        {
            var pockyUser = _pockyUserRepository.GetUser(message.SenderId);
            var limit = _configRepository.GetGeneralConfig("limit");

            if (pockyUser == null || pockyUser.PegsGiven.Count == 0)
            {
                var left = pockyUser?.HasRole(Roles.Unmetered) ?? false ? "unlimited" : limit.ToString();
                return Task.FromResult(new Message
                {
                    Text = $"You have {left} pegs left to give.\n\nYou have not given any pegs so far."
                });
            }

            var groupedPegs = GroupPegsByValidity(pockyUser);
            var validPegs = groupedPegs.ContainsKey(true) ? groupedPegs[true] : new List<Persistence.Models.Peg>();
            var penaltyPegs = groupedPegs.ContainsKey(false) ? groupedPegs[false] : new List<Persistence.Models.Peg>();

            var response = new StringBuilder();

            response.Append(pockyUser.HasRole(Roles.Unmetered)
                ? "You have unlimited pegs left to give."
                : $"You have {limit - validPegs.Count} pegs left to give.");

            if (validPegs.Count > 0)
            {
                response.Append("\n\nHere are the pegs you've given so far:\n");
                foreach (var peg in validPegs)
                {
                    response.Append($"* **{peg.Receiver.Username} — \"_{peg.Comment}_\"\n");
                }
            }

            if (penaltyPegs.Count > 0)
            {
                response.Append("\n\nHere are the penalties you have received:\n");
                foreach (var peg in penaltyPegs)
                {
                    response.Append($"* **{peg.Receiver.Username} — \"_{peg.Comment}_\"\n");
                }
            }

            return Task.FromResult(new Message
            {
                Text = response.ToString()
            });
        }

        private Dictionary<bool, List<Persistence.Models.Peg>> GroupPegsByValidity(PockyUser pockyUser)
        {
            var requireKeywords = _configRepository.GetGeneralConfig("requireValues");
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword").ToArray();

            var groupedPegs = pockyUser.PegsGiven.GroupBy(x =>
                    _pegCommentValidator.IsPegValid(x.Comment, requireKeywords, keywords, penaltyKeywords))
                .ToDictionary(x => x.Key, x => x.ToList());

            return groupedPegs;
        }
    }
}
