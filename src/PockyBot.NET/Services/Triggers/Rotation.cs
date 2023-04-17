using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Triggers
{
    internal class Rotation : ITrigger
    {
        private readonly IConfigRepository _configRepository;
        public string Command => Commands.Rotation;
        public bool DirectMessageAllowed => true;
        public bool CanHaveArgs => false;
        public Role[] Permissions => Array.Empty<Role>();

        public Rotation(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public Task<Message> Respond(Message message)
        {
            var rotation = _configRepository.GetStringConfig("rotation").First();
            var builder = new StringBuilder("## Here's the snack buying rotation:\n\n");

            foreach (var item in rotation.Split(','))
            {
                builder.AppendLine($"* {item}");
            }

            return Task.FromResult(new Message
            {
                Text = builder.ToString()
            });
        }

        public string GetHelpMessage(string botName, PockyUser user)
        {
            return "### How to check the rotation 🔄!\n" +
                   $"1. To check the rotation of teams responsible for buying snacks, type `@{botName} {Commands.Rotation}` OR direct message me with `{Commands.Rotation}`.\n" +
                   "1. I will respond in the room you messaged me in.";
        }
    }
}
