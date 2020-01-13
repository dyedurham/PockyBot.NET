using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using Scriban;

namespace PockyBot.NET.Services.Triggers
{
    internal class Finish : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IPegResultsHelper _pegResultsHelper;
        private readonly IResultsUploader _resultsUploader;
        private readonly IDirectResultsMessageSender _directResultsMessageSender;

        public string Command => Commands.Finish;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => false;

        public string[] Permissions => new[] {Roles.Admin, Roles.Finish};

        public Finish(IPockyUserRepository pockyUserRepository, IPegResultsHelper pegResultsHelper, IResultsUploader resultsUploader, IDirectResultsMessageSender directResultsMessageSender)
        {
            _pockyUserRepository = pockyUserRepository;
            _pegResultsHelper = pegResultsHelper;
            _resultsUploader = resultsUploader;
            _directResultsMessageSender = directResultsMessageSender;
        }

        public async Task<Message> Respond(Message message)
        {
            var users = _pockyUserRepository.GetAllUsersWithPegs();
            var mappedUsers = _pegResultsHelper.MapUsersToPegRecipients(users);

            var winners = _pegResultsHelper.GetWinners(mappedUsers);

            var winnerIds = winners.Select(x => x.UserId);
            var nonWinners = mappedUsers.Where(x => !winnerIds.Contains(x.UserId))
                .OrderBy(x => x.TotalPoints)
                .ToList();

            var categories = _pegResultsHelper.GetCategories(mappedUsers);

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

            var template = GetTemplate();

            var parsedTemplate = Template.Parse(template);
            var html = parsedTemplate.Render(new { model = results });

            var uri = await _resultsUploader.UploadResults(html);
            await _directResultsMessageSender.SendDirectMessagesAsync(mappedUsers);

            return new Message
            {
                Text = $"[Here are all pegs given this cycle]({uri})"
            };
        }

        private string GetTemplate()
        {
            var assembly = typeof(Finish).GetTypeInfo().Assembly;
            using (Stream resource = assembly.GetManifestResourceStream("PockyBot.NET.Resources.results.html"))
            using (StreamReader reader = new StreamReader(resource))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
