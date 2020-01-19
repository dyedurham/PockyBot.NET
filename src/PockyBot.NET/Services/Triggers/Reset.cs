using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Reset : ITrigger
    {
        private readonly IPegRepository _pegRepository;

        public string Command => Commands.Reset;

        public bool DirectMessageAllowed => false;

        public bool CanHaveArgs => false;

        public string[] Permissions => new[] { Roles.Admin, Roles.Reset };

        public Reset(IPegRepository pegRepository)
        {
            _pegRepository = pegRepository;
        }

        public async Task<Message> Respond(Message message)
        {
            try
            {
                await _pegRepository.ClearPegs();
            }
            catch (Exception)
            {
                return new Message
                {
                    Text = "Error clearing pegs."
                };
            }

            return new Message
            {
                Text = "Pegs cleared."
            };
        }
    }
}
