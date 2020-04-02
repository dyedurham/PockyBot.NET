using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Models;
using PockyBot.NET.Services.Pegs;
using Scriban;

namespace PockyBot.NET.Services
{
    internal class ResultsFileGenerator : IResultsFileGenerator
    {
        private readonly IPegResultsHelper _pegResultsHelper;
        private readonly ILogger _logger;
        private readonly IResultsUploader _resultsUploader;

        public ResultsFileGenerator(IPegResultsHelper pegResultsHelper, ILogger logger, IResultsUploader resultsUploader)
        {
            _pegResultsHelper = pegResultsHelper;
            _logger = logger;
            _resultsUploader = resultsUploader;
        }

        public async Task<string> GenerateResultsFileAndReturnLink(List<PegRecipient> mappedUsers)
        {
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

            _logger.LogDebug("Generating HTML...");
            var template = GetTemplate();

            var parsedTemplate = Template.Parse(template);
            var html = parsedTemplate.Render(new { model = results });

            var uri = await _resultsUploader.UploadResults(html);
            return uri;
        }

        private static string GetTemplate()
        {
            var assembly = typeof(ResultsFileGenerator).GetTypeInfo().Assembly;
            using (Stream resource = assembly.GetManifestResourceStream("PockyBot.NET.Resources.results.html"))
            using (StreamReader reader = new StreamReader(resource))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
