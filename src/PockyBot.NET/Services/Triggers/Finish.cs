using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;

namespace PockyBot.NET.Services.Triggers
{
    internal class Finish : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IPegResultsHelper _pegResultsHelper;
        private readonly IDirectResultsMessageSender _directResultsMessageSender;
        private readonly IResultsFileGenerator _resultsFileGenerator;
        private readonly ILogger<Finish> _logger;
        private readonly IUsernameUpdater _usernameUpdater;

        public string Command => Commands.Finish;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => false;

        public Role[] Permissions => new[] {Role.Admin, Role.Finish};

        public Finish(IPockyUserRepository pockyUserRepository, IPegResultsHelper pegResultsHelper,
            IDirectResultsMessageSender directResultsMessageSender, IResultsFileGenerator resultsFileGenerator,
            ILogger<Finish> logger, IUsernameUpdater usernameUpdater)
        {
            _pockyUserRepository = pockyUserRepository;
            _pegResultsHelper = pegResultsHelper;
            _logger = logger;
            _usernameUpdater = usernameUpdater;
            _directResultsMessageSender = directResultsMessageSender;
            _resultsFileGenerator = resultsFileGenerator;
        }

        public async Task<Message> Respond(Message message)
        {
            var users = _pockyUserRepository.GetAllUsersWithPegs();
            users = await _usernameUpdater.UpdateUsernames(users);

            _logger.LogDebug("Mapping users...");
            var mappedUsers = _pegResultsHelper.MapUsersToPegRecipients(users);

            var uri = await _resultsFileGenerator.GenerateResultsFileAndReturnLink(mappedUsers);

            _logger.LogDebug("Sending Direct Messages...");
            await _directResultsMessageSender.SendDirectMessagesAsync(mappedUsers);

            return new Message
            {
                Text = $"[Here are all pegs given this cycle]({uri})"
            };
        }
    }
}
