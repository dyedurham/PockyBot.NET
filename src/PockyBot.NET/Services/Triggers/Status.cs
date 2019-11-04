using System;
using System.Collections.Generic;
using System.Linq;
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
            var limit = _configRepository.GetGeneralConfig("limit") ?? 1;

            if (pockyUser == null || pockyUser.PegsGiven.Count == 0)
            {
                var userPegsLeftText = GetPegsLeftText(pockyUser, limit);
                return Task.FromResult(new Message
                {
                    Text = $"{userPegsLeftText}\n\nYou have not given any pegs so far."
                });
            }

            var groupedPegs = GroupPegsByValidity(pockyUser);
            var validPegs = groupedPegs.ContainsKey(true) ? groupedPegs[true] : new List<Persistence.Models.Peg>();
            var penaltyPegs = groupedPegs.ContainsKey(false) ? groupedPegs[false] : new List<Persistence.Models.Peg>();

            var pegsLeftText = GetPegsLeftText(pockyUser, limit, validPegs.Count);
            var validPegsSentText = GetValidPegsSentText(validPegs);
            var penaltyPegsReceivedText = GetPenaltyPegsReceivedText(penaltyPegs);

            return Task.FromResult(new Message
            {
                Text = $"{pegsLeftText}{validPegsSentText}{penaltyPegsReceivedText}"
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

        private static string GetPegsLeftText(PockyUser pockyUser, int limit, int pegsGiven = 0)
        {
            var amountLeft = pockyUser?.HasRole(Roles.Unmetered) ?? false ? "unlimited" : (limit - pegsGiven).ToString();
            return $"You have {amountLeft} pegs left to give.";
        }

        private static string GetValidPegsSentText(List<Persistence.Models.Peg> validPegs)
        {
            
            if (validPegs.Any())
            {
                var pegsSentList = validPegs.Select(FormatPeg);
                return $"\n\nHere are the pegs you've given so far:\n{string.Join("\n", pegsSentList)}";
            }

            return string.Empty;
        }

        private static string GetPenaltyPegsReceivedText(List<Persistence.Models.Peg> penaltyPegs)
        {

            if (penaltyPegs.Any())
            {
                var pegsSentList = penaltyPegs.Select(FormatPeg);
                return $"\n\nHere are the penalties you have received:\n{string.Join("\n", pegsSentList)}";
            }

            return string.Empty;
        }

        private static string FormatPeg(Persistence.Models.Peg peg)
        {
            return $"* **{peg.Receiver.Username} — \"_{peg.Comment}_\"";
        }
    }
}
