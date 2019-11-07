using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Finish : ITrigger
    {
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IResultsUploader _resultsUploader;

        public string Command => Commands.Finish;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => false;

        public string[] Permissions => new[] {Roles.Admin, Roles.Finish};

        public Finish(IPockyUserRepository pockyUserRepository, IResultsUploader resultsUploader)
        {
            _pockyUserRepository = pockyUserRepository;
            _resultsUploader = resultsUploader;
        }

        public async Task<Message> Respond(Message message)
        {
            var users = _pockyUserRepository.GetAllUsersWithPegs();
        }
    }
}
