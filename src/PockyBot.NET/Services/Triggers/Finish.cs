using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using Scriban;

namespace PockyBot.NET.Services.Triggers
{
    internal class Finish : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IResultsUploader _resultsUploader;
        private readonly IConfigRepository _configRepository;
        private readonly IPegHelper _pegHelper;

        public string Command => Commands.Finish;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => false;

        public string[] Permissions => new[] {Roles.Admin, Roles.Finish};

        public Finish(IPockyUserRepository pockyUserRepository, IResultsUploader resultsUploader, IConfigRepository configRepository, IPegHelper pegHelper)
        {
            _pockyUserRepository = pockyUserRepository;
            _resultsUploader = resultsUploader;
            _configRepository = configRepository;
            _pegHelper = pegHelper;
        }

        public async Task<Message> Respond(Message message)
        {
            var users = _pockyUserRepository.GetAllUsersWithPegs();
            var mappedUsers = MapUsersToPegRecipients(users);

            var winners = GetWinners(mappedUsers);

            var winnerIds = winners.Select(x => x.UserId);
            var nonWinners = mappedUsers.Where(x => !winnerIds.Contains(x.UserId))
                .OrderBy(x => x.TotalPoints)
                .ToList();

            var categories = GetCategories(mappedUsers);

            var penalties = mappedUsers.Where(x => x.PenaltyCount > 0)
                .OrderByDescending(x => x.PenaltyCount)
                .ToList();

            var results = new PegResults
            {
                Date = DateTime.Now,
                Winners = winners,
                PegRecipients = nonWinners,
                Categories = categories,
                Penalties = penalties
            };

            var assembly = typeof(Finish).GetTypeInfo().Assembly;
            string template;
            using (Stream resource = assembly.GetManifestResourceStream("PockyBot.NET.Resources.results.html"))
            using (StreamReader reader = new StreamReader(resource))
            {
                template = reader.ReadToEnd();
            }

            var parsedTemplate = Template.Parse(template);
            var html = parsedTemplate.Render(new {model = results });

            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\results-{DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture)}.html", html);

            var uri = await _resultsUploader.UploadResults(html);

            return new Message
            {
                Text = $"[Here are all pegs given this cycle]({uri})"
            };
        }

        private List<PegRecipient> MapUsersToPegRecipients(List<PockyUser> users)
        {
            var requireKeywords = _configRepository.GetGeneralConfig("requireValues");
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            var penaltyKeywords = _configRepository.GetStringConfig("penaltyKeyword").ToArray();

            return users.Select(x => {
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
            }).ToList();
        }

        private List<PegRecipient> GetWinners(List<PegRecipient> allRecipients)
        {
            var minimum = _configRepository.GetGeneralConfig("minimum") ?? 0;
            var winners = _configRepository.GetGeneralConfig("winners") ?? 3;
            var eligibleWinners = allRecipients.Where(x => x.PegsGivenCount >= minimum);
            var topCutoff = eligibleWinners.Select(x => x.TotalPoints)
                .OrderByDescending(x => x)
                .Take(winners)
                .LastOrDefault();

            return eligibleWinners.Where(x => x.TotalPoints >= topCutoff)
                .OrderBy(x => x.TotalPoints)
                .ToList();
        }

        private List<PegCategory> GetCategories(List<PegRecipient> allRecipients)
        {
            var keywords = _configRepository.GetStringConfig("keyword").ToArray();
            return keywords.Select(x => {
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
                    .OrderByDescending(y => y.TotalPoints)
                    .ToList();
                var topRecipients = categoryRecipients.Where(y => y.TotalPoints  == allRecipients[0].TotalPoints)
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
