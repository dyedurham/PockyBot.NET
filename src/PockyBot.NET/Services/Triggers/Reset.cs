using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Reset : ITrigger
    {
        private readonly IPegRepository _pegRepository;
        private readonly ILogger<Reset> _logger;

        public string Command => Commands.Reset;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => false;

        public Role[] Permissions => new[] { Role.Admin, Role.Reset };

        public Reset(IPegRepository pegRepository, ILogger<Reset> logger)
        {
            _pegRepository = pegRepository;
            _logger = logger;
        }

        public async Task<Message> Respond(Message message)
        {
            try
            {
                _logger.LogInformation("Clearing pegs...");
                await _pegRepository.ClearPegs();
            }
            catch (Exception e)
            {
                _logger.LogError("Error clearing pegs: {@e}", e);
                return new Message
                {
                    Text = "Error clearing pegs."
                };
            }

            _logger.LogInformation("Pegs cleared successfully");
            return new Message
            {
                Text = "Pegs cleared."
            };
        }
    }
}
