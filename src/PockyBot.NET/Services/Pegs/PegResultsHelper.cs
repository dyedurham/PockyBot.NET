using System;
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
            var linkedKeywords = _configRepository.GetStringConfig("linkedKeyword").Select(x => new LinkedKeyword(x)).ToArray();

            return users.Select(x =>
            {
                var receiverLocation = x.Location?.Location;
                var validPegs = GetValidPegs(x.PegsReceived, requireKeywords, keywords, penaltyKeywords,
                    linkedKeywords, receiverLocation);
                var penaltyPegs = GetPenaltyPegs(x.PegsGiven, keywords.Concat(linkedKeywords.Select(k =>  k.Keyword)).ToArray(), penaltyKeywords);

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
                var categoryRecipients = MapRecipientsToCategoryRecipients(allRecipients, x);

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

        private List<PegDetails> GetValidPegs(List<Peg> pegs, int? requireKeywords, string[] keywords,
            string[] penaltyKeywords, LinkedKeyword[] linkedKeywords, string receiverLocation)
        {
            return pegs.Where(y => _pegHelper.IsPegValid(y.Comment, requireKeywords, keywords.Concat(linkedKeywords.Select(x => x.LinkedWord)).ToArray(), penaltyKeywords))
                .Select(y => new PegDetails
                {
                    SenderName = y.Sender.Username,
                    Weight = _pegHelper.GetPegWeighting(y.Sender.Location?.Location, receiverLocation),
                    Comment = y.Comment,
                    Keywords = GetPegKeywords(y.Comment, keywords, linkedKeywords),
                    SenderLocation = y.Sender.Location?.Location
                })
                .ToList();
        }

        private List<PegDetails> GetPenaltyPegs(List<Peg> pegs, string[] keywords, string[] penaltyKeywords)
        {
            return pegs.Where(y => _pegHelper.IsPenaltyPeg(y.Comment, penaltyKeywords, keywords))
                .Select(y => new PegDetails
                {
                    SenderName = y.Receiver.Username,
                    Weight = 1,
                    Comment = y.Comment,
                    Keywords = penaltyKeywords.Where(z => y.Comment.Contains(z, StringComparison.OrdinalIgnoreCase)).ToList(),
                    SenderLocation = y.Receiver.Location?.Location
                })
                .ToList();
        }

        private List<string> GetPegKeywords(string comment, string[] keywords, LinkedKeyword[] linkedKeywords)
        {
            return keywords.Where(x => comment.Contains(x, StringComparison.OrdinalIgnoreCase)).Union(
                linkedKeywords.Where(x => comment.Contains(x.LinkedWord, StringComparison.OrdinalIgnoreCase)).Select(x => x.Keyword)).ToList();
        }

        private static List<PegRecipient> MapRecipientsToCategoryRecipients(List<PegRecipient> allRecipients, string keyword)
        {
            return allRecipients.Select(recipient =>
                {
                    var pegs = recipient.Pegs.Where(z => z.Keywords.Contains(keyword)).ToList();

                    return new PegRecipient
                    {
                        Name = recipient.Name,
                        UserId = recipient.UserId,
                        Location = recipient.Location,
                        TotalPoints = pegs.Count,
                        PegCount = pegs.Count,
                        PenaltyCount = 0,
                        PegsGivenCount = recipient.PegsGivenCount,
                        Pegs = pegs,
                        Penalties = recipient.Penalties
                    };
                })
                .Where(x => x.TotalPoints > 0)
                .OrderByDescending(x => x.TotalPoints)
                .ToList();
        }
    }
}
