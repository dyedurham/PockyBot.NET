using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;

namespace PockyBot.NET.Services.Triggers
{
    internal class Results: ITrigger, IHelpMessageTrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IPegResultsHelper _pegResultsHelper;
        private readonly IResultsFileGenerator _resultsFileGenerator;
        private readonly ILogger _logger;

        public Results(IPockyUserRepository pockyUserRepository, IPegResultsHelper pegResultsHelper,
            IResultsFileGenerator resultsFileGenerator, ILogger<Results> logger)
        {
            _pockyUserRepository = pockyUserRepository;
            _pegResultsHelper = pegResultsHelper;
            _resultsFileGenerator = resultsFileGenerator;
            _logger = logger;
        }

        public string Command => Commands.Results;
        public bool DirectMessageAllowed => false;
        public bool CanHaveArgs => false;
        public Role[] Permissions => new[] { Role.Admin, Role.Results };

        public async Task<Message> Respond(Message message)
        {
            var users = _pockyUserRepository.GetAllUsersWithPegs();
            _logger.LogDebug("Mapping users...");
            var mappedUsers = _pegResultsHelper.MapUsersToPegRecipients(users);

            var uri = await _resultsFileGenerator.GenerateResultsFileAndReturnLink(mappedUsers);

            return new Message
            {
                Text = $"[Here are all pegs given this cycle]({uri})"
            };
        }

        public string GetHelpMessage(string botName, PockyUser user)
        {
            return "### How to display the results 📃!\n" +
                   $"1. To display results, type `@{botName} {Commands.Results}`.\n" +
                   "1. I will respond in the room you messaged me in.";
        }
    }
}
