using System.Collections.Generic;
using System.Linq;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Pegs
{
    internal class PegResultsHelper : IPegResultsHelper
    {
        private readonly IConfigRepository _configRepository;
        private readonly IPegHelper _pegHelper;

        public PegResultsHelper(IConfigRepository configRepository, IPegHelper pegHelper)
        {
            _configRepository = configRepository;
            _pegHelper = pegHelper;
        }

        public List<PegRecipient> MapUsersToPegRecipients(List<PockyUser> users)
        {
            var requireKeywords = _configRepository.GetGeneralConfig("requireValues");
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword").ToArray();

            return users.Select(x =>
            {
                var receiverLocation = x.Location?.Location;
                var validPegs = x.PegsReceived
                    .Where(y => _pegHelper.IsPegValid(y.Comment, requireKeywords, keywords, penaltyKeywords))
                    .Select(y => new PegDetails
                    {
                        SenderName = y.Sender.Username,
                        Weight = _pegHelper.GetPegWeighting(y.Sender.Location?.Location, receiverLocation),
                        Comment = y.Comment,
                        Keywords = keywords.Where(z => y.Comment.Contains(z)).ToList(),
                        SenderLocation = y.Sender.Location?.Location
                    })
                    .ToList();
                var penaltyPegs = x.PegsGiven
                    .Where(y => !_pegHelper.IsPegValid(y.Comment, requireKeywords, keywords, penaltyKeywords))
                    .Select(y => new PegDetails
                    {
                        SenderName = y.Receiver.Username,
                        Weight = 1,
                        Comment = y.Comment,
                        Keywords = penaltyKeywords.Where(z => y.Comment.Contains(z)).ToList(),
                        SenderLocation = y.Receiver.Location?.Location
                    })
                    .ToList();

                return new PegRecipient
                {
                    Name = x.Username,
                    UserId = x.UserId,
                    Location = x.Location?.Location,
                    TotalPoints = validPegs.Sum(y => y.Weight) - penaltyPegs.Count,
                    PegCount = validPegs.Count,
                    PenaltyCount = penaltyPegs.Count,
                    PegsGivenCount = x.PegsGiven.Count - penaltyPegs.Count,
                    Pegs = validPegs,
                    Penalties = penaltyPegs
                };
            }).Where(x => x.PegCount > 0 || x.PenaltyCount > 0)
                .ToList();
        }

        public List<PegRecipient> GetWinners(List<PegRecipient> allRecipients)
        {
            var minimum = _configRepository.GetGeneralConfig("minimum") ?? 0;
            var winners = _configRepository.GetGeneralConfig("winners") ?? 3;
            var eligibleWinners = allRecipients.Where(x => x.PegsGivenCount >= minimum).ToList();
            var topCutoff = eligibleWinners.Select(x => x.TotalPoints)
                .OrderByDescending(x => x)
                .Take(winners)
                .LastOrDefault();

            return eligibleWinners.Where(x => x.TotalPoints >= topCutoff)
                .OrderBy(x => x.TotalPoints)
                .ToList();
        }

        public List<PegCategory> GetCategories(List<PegRecipient> allRecipients)
        {
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            return keywords.Select(x =>
            {
                var categoryRecipients = allRecipients.Select(y =>
                {
                    var pegs = y.Pegs.Where(z => z.Keywords.Contains(x)).ToList();

                    return new PegRecipient
                    {
                        Name = y.Name,
                        UserId = y.UserId,
                        Location = y.Location,
                        TotalPoints = pegs.Count,
                        PegCount = pegs.Count,
                        PenaltyCount = 0,
                        PegsGivenCount = y.PegsGivenCount,
                        Pegs = pegs,
                        Penalties = y.Penalties
                    };
                })
                    .Where(y => y.TotalPoints > 0)
                    .OrderByDescending(y => y.TotalPoints)
                    .ToList();

                if (categoryRecipients.Count == 0)
                {
                    return new PegCategory
                    {
                        Name = x,
                        Recipients = new List<PegRecipient>()
                    };
                }

                var topRecipients = categoryRecipients.Where(y => y.TotalPoints == categoryRecipients[0].TotalPoints)
                    .ToList();

                return new PegCategory
                {
                    Name = x,
                    Recipients = topRecipients
                };
            }).ToList();
        }
    }
}
